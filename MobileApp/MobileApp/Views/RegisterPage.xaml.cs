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
    public partial class RegisterPage : ContentPage
    {
        APIRequestHelper apiRequestHelper;
        DateTime now = DateTime.Now.AddYears(-10); 

        List<string> genderList = new List<string>
        {
            "Male",
            "Female"
        };

        public RegisterPage()
        {
            apiRequestHelper = new APIRequestHelper();
            InitializeComponent();

            var email = new Entry
            {
                Placeholder = "Email",
                Keyboard = Keyboard.Email
            };

            var password = new Entry
            {
                Placeholder = "Password",
                IsPassword = true,
            };

            var confirmPassword = new Entry
            {
                Placeholder = "Confirm Password",
                IsPassword = true,
            };                   

            var firstName = new Entry
            {
                Placeholder = "First Name",
            };

            var lastName = new Entry
            {
                Placeholder = "Last Name",
            };

            var streetName = new Entry
            {
                Placeholder = "Address"
            };

            var district = new Entry
            {
                Placeholder = "District"
            };

            var province = new Entry
            {
                Placeholder = "Province"
            };

            var houseNumber = new Entry
            {
                Placeholder = "House number"                
            };

            Picker genderPicker = new Picker
            {
                Title = "Gender",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            foreach (string gender in genderList)
            {
                genderPicker.Items.Add(gender); 
            }

            Label birthdayLabel = new Label
            {
                Text = "Birthday:"
            };

            DatePicker birhtDay = new DatePicker
            {       
                Format = "D",
                MinimumDate = new DateTime(1950, 1, 1),
                MaximumDate = now, 
                VerticalOptions =LayoutOptions.CenterAndExpand,
                
            };

            var registerButton = new Button
            {
                Text = "Register"
            };

            var stack = new StackLayout
            {
                Padding = 30,
                Spacing = 10,
                Children =
                {
                    new Label {Text = "Register", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), HorizontalOptions = LayoutOptions.Center},
                    email,
                    password,
                    confirmPassword,
                    firstName,
                    lastName,
                    genderPicker,
                    birthdayLabel,
                    birhtDay,
                    streetName,
                    houseNumber,
                    province, 
                    district,
                    registerButton
                }
            };

            Content = new ScrollView { Content = stack }; 

            registerButton.Clicked += async (object sender, EventArgs e) =>
            {
                if (!password.Text.Equals(confirmPassword.Text))
                {
                    await DisplayAlert("Error", "Confirm password doesn't match password", "Cencel");
                }

                await Register(new Client
                {
                    Email = email.Text,
                    Password = password.Text,
                    ConfirmPassword = confirmPassword.Text,
                    FirstName = firstName.Text,
                    LastName = lastName.Text,
                    Gender = genderPicker.SelectedItem.ToString(),
                    BirthDay = birhtDay.Date,
                    StreetName = streetName.Text,
                    HouseNumber = houseNumber.Text,
                    Province = province.Text,
                    District = district.Text,   
                });
            };
        }

        public async Task Register(Client user)
        {
            
            var client = new HttpClient();
            var content = JsonConvert.SerializeObject(user);

            apiRequestHelper.SetTokenHeader();
            var apiResponse = apiRequestHelper.PostRequest(Constants.registerUrl, content);

            if (apiResponse != null)
            {
                Debug.WriteLine(@" User Successfully registered in");
                Debug.WriteLine(apiResponse);
                await Navigation.PushAsync(new SignInPage());
            }
            else
            {
                await DisplayAlert("Invalid login", "The username or password is incorrect.", "Cancel");
                Debug.WriteLine("Er is iets fout gegaan :(");
                Debug.WriteLine(apiResponse);
            }
        }
    
}
}