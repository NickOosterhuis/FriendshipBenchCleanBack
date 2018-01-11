using MobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUserPage : ContentPage
    {
        ClientUser ClientUser;
        public EditUserPage()
        {
            InitializeComponent();
            fetchUser();
        }

        private async Task fetchUser()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(Constants.getUserUrl);
            Debug.WriteLine(response);
            // If the request was succesfull, create a new appointment object
            if (response.IsSuccessStatusCode)
            {
                // Convert the API response into a JSON object.
                String json = await response.Content.ReadAsStringAsync();
                dynamic convertedJson = JsonConvert.DeserializeObject(json);

                // Create a new object from the appointments.
                ClientUser = new ClientUser
                {
                    FirstName = (string)convertedJson.FirstName,
                    LastName = (string)convertedJson.LastName,
                    Gender = (string)convertedJson.Gender,
                    BirthDay = (DateTime)convertedJson.BirthDay,
                    StreetName = (string)convertedJson.StreetName,
                    HouseNumber = (string)convertedJson.HouseNumber,
                    Province = (string)convertedJson.Province,
                    District = (string)convertedJson.District,
                };
            }
            else
            {
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }
        }
    }
}