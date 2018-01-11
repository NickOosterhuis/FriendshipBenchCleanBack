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


namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Signout : ContentPage
    {
        public Signout()
        {
            InitializeComponent();

            var singOutButten = new Button
            {
                Text = "Sign Out"
            };

            Content = new StackLayout
            {
                Padding = 30,
                Spacing = 10,
                Children =
                {
                    new Label {Text = "Sign Out", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), HorizontalOptions = LayoutOptions.Center},
                    singOutButten,
                }
            };

            singOutButten.Clicked += async (object sender, EventArgs e) =>
            {
                await signingOut();
            };
        }

        public async Task signingOut()
        {
            var client = new HttpClient();
            var response = new HttpResponseMessage();
            Debug.WriteLine("Going to log out");

            try
            {
                response = await client.PostAsync(Constants.logoutUrl, null);
                Debug.WriteLine(response);
            }
            catch (Exception e)
            {
                await DisplayAlert("ERROR", e.Message, "Cancel");
                Debug.WriteLine("HTTP ERROR: " + e.Message);
            }

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("succses logedout en go to SignInPage");
                await Navigation.PushAsync(new SignInPage());
            }
        }
    }
}