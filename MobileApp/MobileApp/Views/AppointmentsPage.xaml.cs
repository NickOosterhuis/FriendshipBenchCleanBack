using MobileApp.Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using MobileApp.Helpers;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentsPage : ContentPage
    {
        APIRequestHelper apiRequestHelper;
        ObservableCollection<Appointment> allAppointments;
        ObservableCollection<ObservableGroupCollection<string, Appointment>> groupedAppointments;
        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }

        // Initialize the page.
        public AppointmentsPage()
        {
            apiRequestHelper = new APIRequestHelper();
            BindingContext = this;
            InitializeComponent();
        }

        // Refresh the list with appointments when the user opens this page.
        protected override void OnAppearing()
        {
            base.OnAppearing();
            FetchAppointments();
        }

        // Fetch all the appointments and make a list of it.
        private async Task FetchAppointments()
        {
            // Send token with Http request
            apiRequestHelper.SetTokenHeader();
            // Send a GET request to the API.
            String apiResponse = await apiRequestHelper.GetRequest(Constants.appointmentsUrl);
            if (apiResponse != null)
            {
                // Convert the API response into a JSON object.
                allAppointments = new ObservableCollection<Appointment>();
                dynamic convertedJson = JsonConvert.DeserializeObject(apiResponse);

                // Loop through all the appointments in the JSON object and create appointment objects.
                foreach (var appointment in convertedJson)
                {
                    if (appointment.client.id == (string)App.Current.Properties["id"])
                    {
                        allAppointments.Add(new Appointment
                        {
                            Id = (int)appointment.id,
                            Time = (DateTime)appointment.time,
                            Status = new AppointmentStatus { Id = (int)appointment.status.id, Name = (string)appointment.status.name },
                            Bench = new Bench { Id = (int)appointment.bench.id, Streetname = (string)appointment.bench.streetname, Housenumber = (string)appointment.bench.housenumber, Province = (string)appointment.bench.province, District = (string)appointment.bench.district },
                            Client = new Client { Id = (string)appointment.client.id, Email = (string)appointment.client.email, FirstName = (string)appointment.client.firstname, LastName = (string)appointment.client.lastname, BirthDay = (DateTime)appointment.client.birthDay, District = (string)appointment.client.district, Gender = (string)appointment.client.gender, HouseNumber = (string)appointment.client.houseNumber, Province = (string)appointment.client.province, StreetName = (string)appointment.client.streetName },
                            Healthworker = new Healthworker { Id = (string)appointment.healthworker.id, Firstname = (string)appointment.healthworker.firstname, Lastname = (string)appointment.healthworker.lastname, Birthday = (DateTime)appointment.healthworker.birthday, Gender = (string)appointment.healthworker.gender, Email = (string)appointment.healthworker.email }
                        });
                    }
                }
            }
            else
            {
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }

            // Group and update the list.
            GroupAppointments();
            AppointmentsList.ItemsSource = groupedAppointments;
        }

        // Group appointments by their status.
        private void GroupAppointments()
        {
            // Group the appointments by their status.
            var orderedAppointments =
                allAppointments.OrderBy(a => a.Status.Id)
                .ThenBy(a => a.Time)
                .GroupBy(a => a.Status.Name)
                .Select(a => new ObservableGroupCollection<string, Appointment>(a))
                .ToList();

            // Convert the grouped appointments to a ObservableGroupCollection.
            groupedAppointments = new ObservableCollection<ObservableGroupCollection<string, Appointment>>(orderedAppointments);
        }

        // Show the appointment details when an item has been selected.
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Appointment selectedAppointment = (Appointment)e.SelectedItem;
            Navigation.PushAsync(new AppointmentDetailPage(selectedAppointment.Id));           
        }

        // Refresh the list with appointments when a 'pull-to-refresh' has been performed.
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    await FetchAppointments();
                    IsRefreshing = false;
                });
            }
        }
    }

    // The class for a ObserverableGroupCollection.
    public class ObservableGroupCollection<S, T> : ObservableCollection<T>
    {
        private readonly S _key;
        public ObservableGroupCollection(IGrouping<S, T> group) : base(group)
        {
            _key = group.Key;
        }
        public S Key
        {
            get { return _key; }
        }
    }
}