using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        Label label;
        public Home()
        {
            InitializeComponent();
            Continue.Clicked += OnButtonClicked;
        }


        void OnButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignInPage());
        }
    }
}