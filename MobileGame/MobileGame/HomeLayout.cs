using Xamarin.Forms;

namespace MobileGame
{
    internal class HomeLayout
    {
        //Home Screen
        public HomeLayout(ContentPageController ViewController, bool bypassLogIn)
        {
            HLayout = new StackLayout
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
                    }
                }
            };
            StartButton = new Button
            {
                Text = "Play \u25B6",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof (Button)),
                HorizontalOptions = LayoutOptions.Center,
                //VerticalOptions = LayoutOptions.Fill,
                WidthRequest = 120
            };
            HLayout.Children.Add(StartButton);

            StartButton.Clicked += (sender, args) =>
            {
                ViewController.Invoke(() => StartButton.Text = "Loading \u25B6");
                StartButton.IsEnabled = false;
                if (bypassLogIn)
                {
                    var GSlayout = new GameSelectLayout(ViewController);
                }
                else
                {
                    ViewController.Invoke(() => ViewController.View.Content = ViewController.LogIn.LILayout);
                }
                StartButton.IsEnabled = true;
            };
            Display(ViewController);
        }

        public StackLayout HLayout { get; set; }
        public Button StartButton { get; set; }

        internal void Display(ContentPageController ViewController)
        {
            ViewController.Invoke(() => ViewController.View.Content = HLayout);
        }
    }
}