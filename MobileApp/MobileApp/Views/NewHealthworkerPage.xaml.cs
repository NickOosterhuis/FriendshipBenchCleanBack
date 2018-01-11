using MobileApp.Helpers;
using MobileApp.Models;
using MobileApp.ViewModels;
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
    public partial class NewHealthworkerPage : ContentPage
    {
        APIRequestHelper apiRequestHelper;
        List<Healthworker> healthworkers = new List<Healthworker>();
        int currentHealthworker = 0;

        // Initialize the page.
        public NewHealthworkerPage()
        {
            apiRequestHelper = new APIRequestHelper();
            FetchHealthworker();
            BindingContext = this;
            InitializeComponent();
        }

        // Fetch the Healthworker.
        private async Task FetchHealthworker()
        {
            // Send token with Http request
            apiRequestHelper.SetTokenHeader();
            // Send a GET request to the API.
            string apiResponse = await apiRequestHelper.GetRequest(Constants.healthWorkerUrl);
            if (apiResponse != null)
            {
                // Convert the API response into a JSON object.
                dynamic convertedJson = JsonConvert.DeserializeObject(apiResponse);

                // Create a new object from the appointments.
                foreach (var healthworkerJson in convertedJson)
                {
                    healthworkers.Add(new Healthworker
                    {
                        Id = healthworkerJson.id,
                        Firstname = healthworkerJson.firstname,
                        Lastname = healthworkerJson.lastname,
                        Email = healthworkerJson.email,
                        Birthday = healthworkerJson.birthday,
                        Gender = healthworkerJson.gender,
                        Phonenumber = healthworkerJson.phonenumber
                    });
                }

            }
            else
            {
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }

            // Update the page items.
            BindingContext = healthworkers[currentHealthworker];
        }

        // Actions for when the user wants to see the next healthworker.
        private void next_healthworker(object sender, EventArgs e)
        {
            if (currentHealthworker == (healthworkers.Count - 1))
                currentHealthworker = 0;
            else
                currentHealthworker ++;
            BindingContext = healthworkers[currentHealthworker];
        }

        // Actions for when the user wants to see the previous healthworker.
        private void previous_healthworker(object sender, EventArgs e)
        {
            if (currentHealthworker == 0)
                currentHealthworker = healthworkers.Count - 1;
            else
                currentHealthworker--;

            BindingContext = healthworkers[currentHealthworker];
        }

        // Do a PUT request to update the clients healthworker
        private async Task choose_healthworker(object sender, EventArgs e)
        {
            AddHealthworkerToUserViewModel addHealthworkerModel = new AddHealthworkerToUserViewModel { HealthWorker_Id = healthworkers[currentHealthworker].Id };

            // Do a PUT request.
            string content = JsonConvert.SerializeObject(addHealthworkerModel);
            string response = await apiRequestHelper.PutRequest(Constants.addHealthworkerToClientUrl + "/" + App.Current.Properties["email"], content);
            if (response != null)
            {
                DisplayAlert("Succesfull", healthworkers[currentHealthworker].Firstname + " is your new healthworker.", "Okay");
                App.Current.Properties["healthworker_id"] = healthworkers[currentHealthworker].Id;
                Navigation.PushAsync(new HealthworkerPage());
            }
            else
            {
                // Display an error.
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }
        }
    }
}