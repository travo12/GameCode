using System;
using Xamarin.Forms;

namespace MobileGame
{
    internal class ContentPageController : ContentPage
    {
        public ContentPageController(NavigationDrawer navigationDrawer)
        {
            NavDrawer = navigationDrawer;
            LogIn = new LogInLayout(this);
            LogInManager = new LogInManager();

            // dispite all the work, this never gets seen in release due to real loading times vs emulator loading ties
            var LoadingScreen = new StackLayout
            {
                BackgroundColor = Color.FromHex("1393c3"),
                Children =
                {
                    new Image
                    {
                        Source = "unsafespaceslogo.png"
                    },
                    new Label
                    {
                        Text = "#nooneissafe",
                        FontSize = 20,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        HeightRequest = 25
                    },
                    new Image
                    {
                        Source = "shocked_smile.png"
                    },
                    new ActivityIndicator
                    {
                        Color = Color.Silver,
                        IsRunning = true
                    }
                }
            };

            //Main view inside NavDrawer, wraps the rest of the views inside a scrollview
            View = new ScrollView
            {
                //BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = LoadingScreen
            };

            Content = View;
            // Accomodate iPhone status bar.
            LogIn.LILayout.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
            LogIn.button.Clicked += async (sender, args) =>
            {
                var tempButton = sender as Button;
                Invoke(() => tempButton.IsEnabled = false);

                if ((LogIn.username != null) && (LogIn.password != null))
                {
                    LogIn.button.Text = "Logging In!";

                    //Starts a new Log In
                    await LogInManager.NewLogIn(LogIn, LogIn.Lusername.Text, LogIn.Lpassword.Text, true, this);
                }
                else
                {
                    Invoke(() =>
                        DisplayAlert("Username or Password Blank",
                            "Please fill out username and password before submitting", "OK").ConfigureAwait(false));
                }

                Invoke(() => tempButton.IsEnabled = true);
                LogIn.button.Text = "Click To Log In";
            };
            LogIn.FacebookLogin.Clicked +=
                (object sender, EventArgs e) => { Navigation.PushModalAsync(new LoginPage()); };

            GameManager.ViewController = this;
            // Sends control to LogInManager to handle initial screen
            LogInManager.HandleLogIn(LogIn, this).ConfigureAwait(false);
        }

        public string returnedfromhttp { get; set; }
        //public LeeGame Game { get; set; }
        public LogInLayout LogIn { get; set; }
        //public GameSelectLayout GameSelect { get; set; }
        public ScrollView View { get; set; }
        public string cookiestring { get; set; }
        public LogInManager LogInManager { get; set; }
        public NavigationDrawer NavDrawer { get; set; }

        public void Invoke(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }
    }
}