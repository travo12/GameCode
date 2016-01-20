using Xamarin.Forms;

namespace MobileGame
{
    //Shows screen of players in the game, waiting for the game to start
    internal class GameLobbyLayout
    {
        public GameLobbyLayout(GameLobbyResponse GLResponse, ContentPageController ViewController)
        {
            var Players = GLResponse.Names;
            var Admin = GLResponse.Admin;

            var labelLarge = new Label
            {
                Text = "Waiting Room",
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 80
            };

            adminLabel = new Label
            {
                Text = "GameID = " + GLResponse.GameId,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center
                //IsVisible = false,
            };

            startgamebutton = new Button
            {
                Text = "Start Game",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof (Button)),
                VerticalOptions = LayoutOptions.Fill,
                TranslationY = 2,
                HeightRequest = 55
                //IsVisible = false,
            };
            //view
            GLLayout = new StackLayout
            {
                Children =
                {
                    labelLarge
                    //adminLabel
                    //startgamebutton,
                    //entry,
                    //boxView,
                    //image 
                },
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
                //HeightRequest = 1500
            };
            //Show Start Game Button if Admin
            if (Admin)
            {
                GLLayout.Children.Insert(1, adminLabel);
                GLLayout.Children.Add(startgamebutton);
            }


            foreach (var p in GLResponse.Names)
            {
                GLLayout.Children.Add(new Label {Text = p, HorizontalOptions = LayoutOptions.Center, FontSize = 20});
            }

            startgamebutton.Clicked += async (sender, args) =>
            {
                ViewController.Invoke(() => startgamebutton.IsEnabled = false);
                if (GLResponse.Names.Length >= 3)
                {
                    startgamebutton.Text = "Starting Game";
                    await GameManager.StartGame();
                }
                else
                {
                    ViewController.Invoke(() =>
                        ViewController.DisplayAlert("Not Enough Players", "Need 3 or more players to start.", "OK")
                            .ConfigureAwait(false));
                }
                ViewController.Invoke(() => startgamebutton.IsEnabled = true);
            };

            Display(ViewController);
        }

        public StackLayout GLLayout { get; set; }
        public Button startgamebutton { get; set; }
        public Label adminLabel { get; set; }

        internal void Display(ContentPageController ViewController)
        {
            ViewController.Invoke(() => GameManager.ViewController.View.Content = GLLayout);
        }
    }
}