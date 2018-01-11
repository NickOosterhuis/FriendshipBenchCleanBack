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
    public partial class UserEditPage : ContentPage
    {
        APIRequestHelper apiRequestHelper; 
        DateTime now = DateTime.Now.AddYears(-10);
        Client user; 

        public UserEditPage(Client user)
        {
            apiRequestHelper = new APIRequestHelper();
            InitializeComponent();
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
                UpdateUserAsync(user = new Client
                {
                    Email = emailField.Text,
                    StreetName = streetnameField.Text,
                    HouseNumber = houseNumberField.Text,
                    Province = provinceField.Text,
                    District = districtField.Text,
                });
            };
            editButton.BackgroundColor = (Color)Application.Current.Resources["Primary"];
            editButton.TextColor = Color.White;

            ButtonSpace.Children.Add(editButton);
        }

        private async Task UpdateUserAsync(Client user)
        {
            EditUserViewModel vm = new EditUserViewModel
            {
                Email = user.Email,
                District = user.District,
                StreetName = user.StreetName,
                Province = user.Province,
                HouseNumber = user.HouseNumber
            };

            string content = JsonConvert.SerializeObject(vm);

            apiRequestHelper.SetTokenHeader();
            string response = await apiRequestHelper.PutRequest(Constants.editClientUrl + "/" + user.Email, content);

            if (response != null)
            {
                DisplayAlert("Update message", "User updated successfully", "Okay");
                Navigation.PushAsync(new UserDetailPage());                
            }
            else
            {
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }
        }

    }
}