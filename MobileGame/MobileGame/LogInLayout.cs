using Xamarin.Forms;

namespace MobileGame
{
    internal class LogInLayout
    {
        //Log In Screen
        public LogInLayout(ContentPageController ViewController)
        {
            //Debug variables
            //username = "LimeyJohnson@gmail.com";
            //password = "password123";

            var TitleImage = new Image
            {
                Source = "unsafespaceslogo.png"
            };

            var labelusername = new Label
            {
                Text = "Username:",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Start
            };
            Lusername = new Entry
            {
                //Placeholder = username,
                VerticalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Text
            };
            var labelpassword = new Label
            {
                Text = "Password:",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Start
            };
            Lpassword = new Entry
            {
                //Placeholder = password,
                VerticalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Text,
                IsPassword = true
            };

            button = new Button
            {
                Text = "Click to Login",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof (Button)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill
                //HeightRequest = 75,
            };
            FacebookLogin = new Button
            {
                Text = "Login with Facebook",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof (Button)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill
                //HeightRequest = 75,
            };
            labelstr = new Label
            {
                Text = "Not Yet connected!",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };


            LILayout = new StackLayout
            {
                Children =
                {
                    TitleImage,
                    labelusername,
                    Lusername,
                    labelpassword,
                    Lpassword,
                    button,
                    FacebookLogin
                    //labelstr,
                }
                //HeightRequest = 1500
            };
        }

        public string username { get; set; }
        public string password { get; set; }
        public StackLayout LILayout { get; set; }
        public Label labelstr { get; set; }
        public Button button { get; set; }
        public Entry Lusername { get; set; }
        public Entry Lpassword { get; set; }
        public Button FacebookLogin { get; set; }
    }
}