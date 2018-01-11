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

    //Zorgen dat we de client gegevens kunnen ophalen.
    //Zorgen dat we aan de hand van de client de healthworker ID kunnen ophalen (MOET NOG STRING WORDEN!!).
    //Aan de hand van de healthworker de onderstaande gegevens invullen.

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthworkerPage : ContentPage
    {
        APIRequestHelper apiRequestHelper;
        Healthworker healthworker;

        // Initialize the page.
        public HealthworkerPage()
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
            string apiResponse = await apiRequestHelper.GetRequest(Constants.healthWorkerUrl + "/" + App.Current.Properties["healthworker_id"]);
            if (apiResponse != null)
            {
                // Convert the API response into a JSON object.
                dynamic convertedJson = JsonConvert.DeserializeObject(apiResponse);

                // Create a new object from the appointments.
                healthworker = new Healthworker
                {
                    Id = (string)App.Current.Properties["healthworker_id"],
                    Firstname = convertedJson.firstname,
                    Lastname = convertedJson.lastname,
                    Email = convertedJson.email,
                    Birthday = convertedJson.birthday,
                    Gender = convertedJson.gender,
                    Phonenumber = convertedJson.phoneNumber
                };
            }
            else
            {
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }

            // Update the page items.
            BindingContext = healthworker;
        }
    }
}