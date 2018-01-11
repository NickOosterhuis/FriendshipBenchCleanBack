using MobileApp.Helpers;
using MobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LandingPage : MasterDetailPage
	{
        APIRequestHelper apiRequestHelper;

        public LandingPage ()
		{
			InitializeComponent ();
            List<NavigationItem> navigationItems = new List<NavigationItem>();
            navigationItems.Add(new NavigationItem { Name = "Questionnaires", TargetType = typeof(MainQuestionnaire) });
            navigationItems.Add(new NavigationItem { Name = "Healthworker", TargetType = typeof(HealthworkerPage) });
            navigationItems.Add(new NavigationItem { Name = "Appointments", TargetType = typeof(AppointmentsPage) });
            navigationItems.Add(new NavigationItem { Name = "Profile", TargetType = typeof(UserDetailPage) });
            navigationItems.Add(new NavigationItem { Name = "About", TargetType = typeof(AboutPage) });
            NavigationList.ItemsSource = navigationItems;
            NavigationList.ItemSelected += OnItemSelected;
            Detail = new NavigationPage(new AboutPage());
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as NavigationItem;
            if(item.Name == "Healthworker")
            {
                
                if(App.Current.Properties["healthworker_id"] == null)
                {

                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(NewHealthworkerPage)));
                }
                else
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HealthworkerPage)));
                }
            } else
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
            }
            IsPresented = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SaveUserId();
        }

        async Task SaveUserId()
        {
            apiRequestHelper = new APIRequestHelper();
            string email = App.Current.Properties["email"] as string;
            
            // Set JWT token in header
            apiRequestHelper.SetTokenHeader();
            // Do getRequest
            string apiResponse = await apiRequestHelper.GetRequest(Constants.getCurrentUserUrl + "/" + email);
            dynamic convertedJson = JsonConvert.DeserializeObject(apiResponse);
            App.Current.Properties["id"] = (string)convertedJson.id;
            App.Current.Properties["healthworker_id"] = (string)convertedJson.healthWorker_Id;
        }
    }

    public class NavigationItem
    {
        public string Name { get; set; }
        
        public string Icon { get; set; }

        public Type TargetType { get; set; }
    }

}