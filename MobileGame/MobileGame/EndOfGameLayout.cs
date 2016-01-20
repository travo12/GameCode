using System.Linq;
using Xamarin.Forms;

namespace MobileGame
{
    //displays End of Game Screen
    internal class EndOfGameLayout
    {
        public EndOfGameLayout(EndOfGameResponse EOGResponse, ContentPageController ViewController)
        {
            var Input = EOGResponse.PlayerInputs;
            var PlayerGames = EOGResponse.PlayerGames;

            var labelLarge = new Label
            {
                Text = "Final Results",
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 55
            };

            EndButton = new Button
            {
                Text = "End Game",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof (Button)),
                VerticalOptions = LayoutOptions.Fill,
                //TranslationY = 2,
                HeightRequest = 55
                //IsVisible = false,
            };

            EOGLayout = new StackLayout
            {
                Children =
                {
                    labelLarge
                },
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };
            // Show Final Points
            foreach (var p in PlayerGames.OrderByDescending(pg => pg.Points).ToArray())
            {
                EOGLayout.Children.Add(new Label
                {
                    Text = p.Name + ": " + p.Points,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 15
                });
            }

            EOGLayout.Children.Add(EndButton);

            var TotalLabel = new Label
            {
                Text = "Round 15 Results",
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 55
            };
            EOGLayout.Children.Add(TotalLabel);

            //order players by points, then display
            foreach (var p in Input)
            {
                EOGLayout.Children.Add(new Label
                {
                    Text = p.Name + ": " + p.Score,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 15
                });
            }

            var VoteStatsImage = new Image
            {
                Source = "statistics.png"
            };
            EOGLayout.Children.Add(VoteStatsImage);

            //Show who voted for what
            foreach (var p in Input)
            {
                EOGLayout.Children.Add(new Label
                {
                    Text = p.Name + "'s Answer:   " + p.Answer,
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.Center
                });

                var tempInputs = Input.Where(pi => pi.Vote == p.PlayerId).Select(pi => pi.Name);
                var whoVoted = string.Join(", ", tempInputs);


                EOGLayout.Children.Add(new Label
                {
                    Text = "Votes: " + whoVoted,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 10,
                    HeightRequest = 40
                });
            }


            EndButton.Clicked += (sender, args) =>
            {
                ViewController.Invoke(() => EndButton.IsEnabled = false);
                //nextRoundButon.Text = "starting next round";
                GameManager.EndGame();
                ViewController.Invoke(() => EndButton.IsEnabled = false);
            };


            Display(ViewController);
        }

        public StackLayout EOGLayout { get; set; }
        public Button EndButton { get; set; }

        internal void Display(ContentPageController ViewController)
        {
            ViewController.Invoke(() => { ViewController.View.Content = EOGLayout; });
        }
    }
}