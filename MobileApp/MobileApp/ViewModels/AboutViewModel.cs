using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace MobileApp
{
    public class AboutViewModel 
    {
        public AboutViewModel()
        {
            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://www.friendshipbenchzimbabwe.org/")));
        }

        public ICommand OpenWebCommand { get; }
    }
}