using Xamarin.Forms;

namespace MobileGame
{
    internal class GameSelectLayout
    {
        //Layout for game selection
        public GameSelectLayout(ContentPageController ViewController)
        {
            //GameID = "641b0eda";
            ViewController.NavDrawer.ShowLayoutMenu();
            var SelectGame = new Image
            {
                Source = "select_game.png"
            };
            var gamelabel = new Label
            {
                Text = "Enter Game Number:",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            var GameNumberEntry = new Entry
            {
                //Placeholder = GameID,
                VerticalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Text,
                //HeightRequest = 75
                WidthRequest = 180
            };

            gamebutton = new Button
            {
                Text = "Play \u25B6",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof (Button)),
                HorizontalOptions = LayoutOptions.End,
                //VerticalOptions = LayoutOptions.Fill,
                WidthRequest = 120
                //HeightRequest = 50,
                //TranslationY= 15,
            };

            newgamebutton = new Button
            {
                Text = "Create New Game \u25B6",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof (Button)),
                VerticalOptions = LayoutOptions.Fill,
                TranslationY = 2,
                HeightRequest = 55
            };

            var RecentImage = new Image
            {
                Source = "recent_games.png"
            };

            InfoLabel = new Label
            {
                Text = "Gathering Recent Games...",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            GSLayout = new StackLayout
            {
                Children =
                {
                    SelectGame,
                    gamelabel,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            GameNumberEntry,
                            gamebutton
                        }
                    },
                    newgamebutton,
                    RecentImage,
                    InfoLabel
                },
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            gamebutton.Clicked += async (sender, args) =>
            {
                ViewController.Invoke(() => gamebutton.IsEnabled = false);
                if (GameNumberEntry.Text != null)
                {
                    //Pass control on to Game from here on out
                    gamebutton.Text = "Joining";
                    //DEBUG
                    //await GameManager.JoinGame(this.GameID); 

                    //RELEASE
                    await GameManager.JoinGame(GameNumberEntry.Text);
                    ViewController.Invoke(() => gamebutton.IsEnabled = true);
                }
                else
                {
                    ViewController.Invoke(() =>
                        ViewController.DisplayAlert("Nothing in Answer",
                            "Don't forget to input answer before submitting.", "OK").ConfigureAwait(false));
                }
            };

            newgamebutton.Clicked += async (sender, args) =>
            {
                //Pass control on to Game from here on out
                ViewController.Invoke(() => newgamebutton.IsEnabled = false);
                await GameManager.CreateGame();
                ViewController.Invoke(() => newgamebutton.IsEnabled = false);
            };

            GameManager.GetActiveGames(this).ConfigureAwait(false);

            Display();
        }

        public string GameID { get; set; }
        public Button gamebutton { get; set; }
        public Button newgamebutton { get; set; }
        public Label desclabel { get; set; }
        public StackLayout GSLayout { get; set; }
        public ListView GSListView { get; set; }
        public Label InfoLabel { get; set; }

        internal void addActiveGames(GetActiveGamesResponse[] gameList, ContentPageController ViewController)
        {
            GSListView = new ListView
            {
                ItemsSource = gameList,
                ItemTemplate = new DataTemplate(() =>
                {
                    var gameIDlabel = new Label();
                    gameIDlabel.SetBinding(Label.TextProperty, "GameId");

                    gameIDlabel.FontSize = 10;
                    gameIDlabel.MinimumWidthRequest = 80;

                    var dateLabel = new Label();
                    dateLabel.SetBinding(Label.TextProperty, "CreatedDate");
                    dateLabel.FontSize = 10;

                    var AdminLabel = new Label();
                    AdminLabel.SetBinding(Label.TextProperty, "AdminName");
                    AdminLabel.FontSize = 10;


                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                gameIDlabel,
                                dateLabel,
                                AdminLabel
                            }
                        }
                    };
                })
            };
            GSLayout.Children.Remove(InfoLabel);
            GSLayout.Children.Add(GSListView);
            GSListView.ItemSelected += async (sender, args) =>
            {
                var responses = (GetActiveGamesResponse) args.SelectedItem;

                //This needs to call the refresh button
                await GameManager.JoinGame(responses.GameId);
            };
        }

        internal void Display()
        {
            GameManager.ViewController.Invoke(() => GameManager.ViewController.View.Content = GSLayout);
        }
    }
}