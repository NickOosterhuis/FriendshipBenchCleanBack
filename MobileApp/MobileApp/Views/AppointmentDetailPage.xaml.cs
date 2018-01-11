using MobileApp.Helpers;
using MobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentDetailPage : ContentPage
    {
        APIRequestHelper apiRequestHelper;
        Appointment appointment;
        int appointmentId;

        // Initialize the page.
        public AppointmentDetailPage(int appointmentId)
        {
            apiRequestHelper = new APIRequestHelper();
            this.appointmentId = appointmentId;
            FetchAppointment();
            InitializeComponent();
        }

        // Fetch the appointment.
        private async Task FetchAppointment()
        {
            // Send token with Http request
            apiRequestHelper.SetTokenHeader();
            // Send a GET request to the API.
            string apiResponse = await apiRequestHelper.GetRequest(Constants.appointmentsUrl + "/" + appointmentId);
            if (apiResponse != null)
            {
                // Convert the API response into a JSON object.
                dynamic convertedJson = JsonConvert.DeserializeObject(apiResponse);

                // Create a new object from the appointments.
                appointment = new Appointment
                {
                    Id = (int)convertedJson.id,
                    Time = (DateTime)convertedJson.time,
                    Status = new AppointmentStatus { Id = (int)convertedJson.status.id, Name = (string)convertedJson.status.name },
                    Bench = new Bench { Id = (int)convertedJson.bench.id, Streetname = (string)convertedJson.bench.streetname, Housenumber = (string)convertedJson.bench.housenumber, Province = (string)convertedJson.bench.province, District = (string)convertedJson.bench.district },
                    Client = new Client { Id = (string)convertedJson.client.id, Email = (string)convertedJson.client.email, FirstName = (string)convertedJson.client.firstname, LastName = (string)convertedJson.client.lastname, District = (string)convertedJson.client.district, Gender = (string)convertedJson.client.gender, HouseNumber = (string)convertedJson.client.houseNumber, Province = (string)convertedJson.client.province, StreetName = (string)convertedJson.client.streetName },
                    Healthworker = new Healthworker { Id = (string)convertedJson.healthworker.id, Firstname = (string)convertedJson.healthworker.firstname, Lastname = (string)convertedJson.healthworker.lastname, Birthday = (DateTime)convertedJson.healthworker.birthday, Gender = (string)convertedJson.healthworker.gender, Email = (string)convertedJson.healthworker.email }
                };
            }
            else
            {
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }

            // Update the page items.
            BindingContext = appointment;
            UpdateButtons();
        }

        // Show the right buttons for this appointment.
        private void UpdateButtons()
        {
            // Remove existing buttons.
            ButtonSpace.Children.Clear();

            // Create a accept and a cancel button.
            Button acceptButton = new Button { Text = "Accept the appointment" };
            Button cancelButton = new Button { Text = "Cancel the appointment" };

            // Add listeners to these buttons.
            acceptButton.Clicked += (object sender, EventArgs e) => {
                UpdateAppointmentStatusAsync(appointment, true);
            };
            acceptButton.BackgroundColor = (Color)Application.Current.Resources["Primary"];
            acceptButton.TextColor = Color.White;
            cancelButton.Clicked += (object sender, EventArgs e) => {
                UpdateAppointmentStatusAsync(appointment, false);
            };

            // If the appointment is waiting for response, show an accept and a cancel button.
            if (appointment.Status.Id == 1)
            {
                ButtonSpace.Children.Add(acceptButton);
                ButtonSpace.Children.Add(cancelButton);
            }
            // If the appointment has been accepted, show a cancel button.
            else if (appointment.Status.Id == 2)
            {
                ButtonSpace.Children.Add(cancelButton);
            }
        }

        // Update the status of an appointment.
        private async Task UpdateAppointmentStatusAsync(Appointment appointment, bool accepted)
        {
            // Ask for a confirmation when the user wants to cancel the appointment.
            if(!accepted)
            {
                bool answer = await DisplayAlert("Warning", "Are you sure you want to cancel the appointment?", "yes", "no");
                if (!answer)
                {
                    return;
                }
            }

            // Create a model that will be sent to update through the API
            int statusId = accepted ? 2 : 3;
            AppointmentViewModel viewModel = new AppointmentViewModel
            {
                Id = appointment.Id,
                Time = appointment.Time,
                StatusId = statusId,
                BenchId = appointment.Bench.Id,
                ClientId = appointment.Client.Id,
                HealthworkerId = appointment.Healthworker.Id,
            };

            // Do a PUT request.
            string content = JsonConvert.SerializeObject(viewModel);
            // Send token with Http request
            apiRequestHelper.SetTokenHeader();
            string response = await apiRequestHelper.PutRequest(Constants.appointmentsUrl + "/" + appointment.Id, content);
            if (response != null)
            {
                // Display a succesfull alert.
                String alertTitle = accepted ? "Appointment accepted" : "Appointment canceled";
                String alertMessage = accepted ? "The appointment has been accepted." : "The appointment has been canceled.";
                DisplayAlert(alertTitle, alertMessage, "Okay");
                FetchAppointment();
            } 
            else
            {
                // Display an error.
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }

        }
    }
}