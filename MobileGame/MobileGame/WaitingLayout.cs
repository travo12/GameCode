using Xamarin.Forms;

namespace MobileGame
{
    internal class WaitingLayout
    {
        //Waiting View
        public WaitingLayout(WaitingResponse WResponse, ContentPageController ViewController)
        {
            var PlayersReady = WResponse.Players;
            var RoundNumber = WResponse.RoundNumber;

            var RoundLabel = new Label
            {
                Text = "Round " + RoundNumber,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 100
            };

            var labelLarge = new Label
            {
                Text = "Waiting on others",
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 50
            };

            WaitLayout = new StackLayout
            {
                Children =
                {
                    RoundLabel,
                    labelLarge
                },
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };
            foreach (var R in PlayersReady)
            {
                // can be Buttons, ListView, Picker
                WaitLayout.Children.Add(new Label
                {
                    Text = R.Name + ": " + R.Status,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 15
                });
            }
            Display(ViewController);
        }

        public StackLayout WaitLayout { get; set; }

        internal void Display(ContentPageController ViewController)
        {
            ViewController.Invoke(() => { ViewController.View.Content = WaitLayout; });
        }
    }
}