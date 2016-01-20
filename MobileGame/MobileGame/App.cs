using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MobileGame
{
    public class App : Application
    {
        public App()
        {
            Instance = this;
            // The root page of your application
            try
            {
                MainPage = new NavigationDrawer();
            }
            catch (Exception e)
            {
                Debug.WriteLine("this happened ->" + e.Message);
            }
        }

        public static App Instance { get; private set; }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public void SuccessfulLoginAction(string token)
        {
            var loginManager = new LogInManager();
            loginManager.LoginFacebook(token).Wait();
            MainPage = new NavigationDrawer();
        }
    }
}