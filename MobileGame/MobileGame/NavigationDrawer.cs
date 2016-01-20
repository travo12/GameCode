using Xamarin.Forms;

namespace MobileGame
{
    internal class NavigationDrawer : MasterDetailPage // Navigation Drawer using MasterDetailPage
    {
        private const string HowToPlayOne =
            "This game requires 3 or more players to start the game. The game has 15 rounds. Each round a question will appear. Come up with a creative answer and submit.";

        private const string HowToPlayTwo =
            "Once all responses are submitted, players will vote for their favorite, without knowing who the author is.";

        private const string HowToPlayThree =
            "Whoever gets the most votes, gains points from the round.Whoever has the most points at the end of 15 rounds wins the game.";

        public NavigationDrawer()
        {
            Title = "Lee Game";
            ViewController = new ContentPageController(this);
            string[] temp = {"Refresh Game", "Switch Games", "How To Play", "Logout"};

            listView = GetListView(temp);


            MasterPage = new ContentPage
            {
                Title = "Options",
                Content = listView,
                Icon = "hamburger.png",
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            Master = MasterPage;

            MasterPage.IsVisible = false;
            //menu buttons & handlers
            listView.ItemTapped += async (sender, e) =>
            {
                switch (e.Item.ToString())
                {
                    case "Refresh Game":
                        if (InGame) await GameManager.RefreshGame().ConfigureAwait(false); //.ConfigureAwait(false);
                        ViewController.Invoke(() => IsPresented = false);
                        break;
                    case "Switch Games":
                        var GameSelect = new GameSelectLayout(ViewController);
                        ViewController.Invoke(() => IsPresented = false);
                        break;
                    case "Logout":
                        ViewController.LogInManager.LogOut();
                        ViewController.Invoke(() => IsPresented = false);
                        break;
                    case "How To Play":
                        ViewController.Invoke(() => ((ListView) sender).SelectedItem = null);
                        var OkButton = new Button
                        {
                            Text = "Back To Menu",
                            FontSize = 15,
                            HorizontalOptions = LayoutOptions.Center,
                            HeightRequest = 50
                        };
                        var tempstack = new StackLayout
                        {
                            Children =
                            {
                                /*
                                new Label
                                {
                                     Text = "How To Play",
                                     FontSize = 30,
                                     HorizontalOptions = LayoutOptions.Center,
                                     HeightRequest = 50,
                                },
                                */
                                new Image
                                {
                                    Source = "how_to_play.png"
                                },
                                new Label
                                {
                                    Text = HowToPlayOne,
                                    FontSize = 18,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = HowToPlayTwo,
                                    FontSize = 18,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = HowToPlayThree,
                                    FontSize = 18,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                OkButton
                            }
                        };

                        ViewController.Invoke(() => MasterPage.Content = tempstack);

                        OkButton.Clicked +=
                            (senderer, args) => { ViewController.Invoke(() => MasterPage.Content = listView); };
                        break;
                    default:
                        ViewController.Invoke(() => IsPresented = false);
                        break;
                }
                ViewController.Invoke(() => ((ListView) sender).SelectedItem = null);
            };


            Detail = new NavigationPage(ViewController);
        }

        public ContentPageController ViewController { get; set; }
        public ListView listView { get; set; }
        public bool InGame { get; set; }
        public ContentPage MasterPage { get; set; }

        public ListView GetListView(string[] listNames)
        {
            var LView = new ListView
            {
                ItemsSource = listNames,
                RowHeight = 100
            };

            return LView;
        }

        public void ShowMenu()
        {
            InGame = true;
            ViewController.Invoke(() => Master.IsVisible = true);
        }

        public void ShowLayoutMenu()
        {
            InGame = false;
            ViewController.Invoke(() => Master.IsVisible = true);
        }

        public void HideMenu()
        {
            ViewController.Invoke(() => Master.IsVisible = false);
        }
    }
}