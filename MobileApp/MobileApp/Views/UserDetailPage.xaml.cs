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
    public partial class UserDetailPage : ContentPage
    {
        Client user;
        APIRequestHelper apiRequestHelper;

        public UserDetailPage()
        {
            apiRequestHelper = new APIRequestHelper();
            InitializeComponent();
            GetCurrentUser();
        }

        //Fetch the current user
        public async Task GetCurrentUser()
        {
            string email = App.Current.Properties["email"] as string;
            var token = App.Current.Properties["token"] as string;

            Debug.WriteLine("Credentials from memory!!!: " + email + " " + token);

            apiRequestHelper.SetTokenHeader();
            string apiResponse = await apiRequestHelper.GetRequest(Constants.getCurrentUserUrl + "/" + email);

            Debug.WriteLine("API RESPONSE IST JETZT HIER!" + apiResponse);

            if (apiResponse != null)
            {
                dynamic convertedJson = JsonConvert.DeserializeObject(apiResponse);

                user = new Client
                {
                    Email = (string)convertedJson.email,
                    FirstName = (string)convertedJson.firstName,
                    LastName = (string)convertedJson.lastName,
                    Gender = (string)convertedJson.gender,
                    BirthDay = (DateTime)convertedJson.birthDay,
                    StreetName = (string)convertedJson.streetName,
                    District = (string)convertedJson.district,
                    Province = (string)convertedJson.province,
                    HouseNumber = (string)convertedJson.houseNumber,
                };
            }
            else
            {
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }

            Debug.WriteLine("USER DATA IST JETZT HIERRR!" + user);

            BindingContext = user;
            UpdateButtons();
        }               

        private void UpdateButtons()
        {
            // Remove existing buttons.
            ButtonSpace.Children.Clear();

            // Create a accept and a cancel button.
            Button editButton = new Button { Text = "Edit" };


            // Add listeners to these buttons.
            editButton.Clicked += (object sender, EventArgs e) =>
            {
                Navigation.PushAsync(new UserEditPage(user));
            };
            editButton.BackgroundColor = (Color)Application.Current.Resources["Primary"];
            editButton.TextColor = Color.White;

            ButtonSpace.Children.Add(editButton);
        }
    }
}