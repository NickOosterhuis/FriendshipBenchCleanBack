using MobileApp.Helpers;
using MobileApp.Models;
using MobileApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainQuestionnaire : ContentPage
	{
        APIRequestHelper apiRequestHelper;
        ObservableCollection<QuestionnaireViewModel> allQuestionnaires;
        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }

        // Initialize the page.
        public MainQuestionnaire()
        {
            apiRequestHelper = new APIRequestHelper();
            BindingContext = this;
            InitializeComponent();
        }

        // Refresh the list with questionnaires when the user opens this page.
        protected override void OnAppearing()
        {
            base.OnAppearing();
            FetchQuestionnaires();
        }

        // Fetch all the questionnaires and make a list of it.
        private async Task FetchQuestionnaires()
        {
            // Send JWT Token With HTTPheader
            apiRequestHelper.SetTokenHeader();
            // Send a GET request to the API.
            String apiResponse = await apiRequestHelper.GetRequest(Constants.questionnaireUrl);
            if (apiResponse != null)
            {
                // Convert the API response into a JSON object.
                allQuestionnaires = new ObservableCollection<QuestionnaireViewModel>();
                dynamic convertedJson = JsonConvert.DeserializeObject(apiResponse);

                // Loop through all the appointments in the JSON object and create appointment objects.
                foreach (var questionnaire in convertedJson)
                {
                    if (questionnaire.client_id == (string)App.Current.Properties["id"])
                    {
                        allQuestionnaires.Add(new QuestionnaireViewModel
                        {
                            Id = (int)questionnaire.id,
                            Time = (DateTime)questionnaire.time,
                            Redflag = (bool)questionnaire.redflag
                        });
                    }
                }
            }
            else
            {
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }

            QuestionnaireList.ItemsSource = allQuestionnaires;
        }

        // Show the appointment details when an item has been selected.
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            QuestionnaireViewModel selectedQuestionnaire = (QuestionnaireViewModel)e.SelectedItem;
            Navigation.PushAsync(new QuestionnaireDetailPage(selectedQuestionnaire.Id));
        }

        // Start a new questionnaire.
        private void start_test(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestionnairePage());
        }

        // Refresh the list with appointments when a 'pull-to-refresh' has been performed.
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    await FetchQuestionnaires();
                    IsRefreshing = false;
                });
            }
        }
    }
}