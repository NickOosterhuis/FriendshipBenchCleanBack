using MobileApp.Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using MobileApp.Helpers;
using MobileApp.ViewModels;

namespace MobileApp.Views
{    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        public Picker MenuItem;
        APIRequestHelper apiRequestHelper;
        Client client; 
        

        public SignInPage()
        {
            InitializeComponent();
            apiRequestHelper = new APIRequestHelper(); 

            RegisterButton.Clicked += (object sender, EventArgs e) =>
            {
                Navigation.PushAsync(new RegisterPage());
            };

            SignInButton.Clicked += async (object sender, EventArgs e) =>
            {
                client = new Client { Email = Email.Text, Password = Password.Text };
                await Login(client);
            };
        }


        //Method to save logged in user credentials in local app storage
        protected async override void OnDisappearing()
        {

            if (Email.Text != null && Password.Text != null)
            {
                LoginViewModel vm = new LoginViewModel
                {
                    Email = Email.Text,
                    Password = Password.Text,
                };


                var content = JsonConvert.SerializeObject(vm);
                var tokenJson = await apiRequestHelper.GetAccessToken(content);

                dynamic token = JsonConvert.DeserializeObject(tokenJson);

                Debug.WriteLine((string)token.token);

                App.Current.Properties["email"] = client.Email;
                App.Current.Properties["token"] = (string)token.token;
                App.Current.Properties["password"] = client.Password;

                App.Current.SavePropertiesAsync();

            }
                        
        }

        public async Task Login(Client user)
       {
            LoginViewModel vm = new LoginViewModel
            {
                Email = user.Email,
                Password = user.Password
            };

            var content = JsonConvert.SerializeObject(vm); 
            
            var apiResponse = await apiRequestHelper.PostRequest(Constants.loginUrl, content);

            if (apiResponse != null)
            {
                Debug.WriteLine(@" User Successfully logged in");
                await Navigation.PushAsync(new LandingPage());
            }
            else
            {
                await DisplayAlert("Invalid login", "The username or password is incorrect.", "Cancel");
                Debug.WriteLine("Er is iets fout gegaan :("); 
            }  
        }
    }
}