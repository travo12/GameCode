using System.Linq;
using Xamarin.Forms;

namespace MobileGame
{
    internal class ResultsLayout
    {
        //Shows Results Page
        public ResultsLayout(ResultsResponse RResponse, ContentPageController ViewController)
        {
            var Input = RResponse.PlayerInputs;
            var PlayerGames = RResponse.PlayerGames;
            var roundnumber = RResponse.RoundNumber;
            var Admin = RResponse.Admin;


            var RoundLabel = new Label
            {
                Text = "Round " + roundnumber,
                FontSize = 35,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 55
            };

            var ResultsImage = new Image
            {
                Source = "results.png"
            };

            nextRoundButton = new Button
            {
                Text = "Next Round",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof (Button)),
                VerticalOptions = LayoutOptions.Fill,
                //TranslationY = 2,
                HeightRequest = 55
                //IsVisible = false,
            };

            ResLayout = new StackLayout
            {
                Children =
                {
                    RoundLabel,
                    ResultsImage
                },
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            foreach (var p in Input)
            {
                ResLayout.Children.Add(new Label
                {
                    Text = p.Name + ": " + p.Score,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 15
                });
            }

            if (Admin)
            {
                ResLayout.Children.Add(nextRoundButton);
            }

            var TotalScoreImage = new Image
            {
                Source = "total_score.png"
            };
            ResLayout.Children.Add(TotalScoreImage);

            //order players by points, then display
            foreach (var p in PlayerGames.OrderByDescending(pg => pg.Points).ToArray())
            {
                ResLayout.Children.Add(new Label
                {
                    Text = p.Name + ": " + p.Points,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 15
                });
            }

            var VoteStatsImage = new Image
            {
                Source = "statistics.png"
            };
            ResLayout.Children.Add(VoteStatsImage);

            foreach (var p in Input)
            {
                ResLayout.Children.Add(new Label
                {
                    Text = p.Name + "'s Answer:   " + p.Answer,
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.Center
                });

                var tempInputs = Input.Where(pi => pi.Vote == p.PlayerId).Select(pi => pi.Name);
                var whoVoted = string.Join(", ", tempInputs);


                ResLayout.Children.Add(new Label
                {
                    Text = "Votes: " + whoVoted,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 10,
                    HeightRequest = 40
                });
            }


            nextRoundButton.Clicked += async (sender, args) =>
            {
                ViewController.Invoke(() => nextRoundButton.IsEnabled = false);
                //nextRoundButon.Text = "starting next round";
                await GameManager.NewRound();
                ViewController.Invoke(() => nextRoundButton.IsEnabled = false);
            };


            Display(ViewController);
        }

        public StackLayout ResLayout { get; set; }
        public Button nextRoundButton { get; set; }

        internal void Display(ContentPageController ViewController)
        {
            ViewController.Invoke(() => { ViewController.View.Content = ResLayout; });
        }

    }
}