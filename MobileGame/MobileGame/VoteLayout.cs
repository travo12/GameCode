using Xamarin.Forms;

namespace MobileGame
{
    internal class VoteLayout
    {
        // Voting Page
        public VoteLayout(VoteResponse VResponse, ContentPageController ViewController)
        {
            var inputs = VResponse.PlayerAnswers;
            var RoundNumber = VResponse.RoundNumber;

            var RoundLabel = new Label
            {
                Text = "Round " + RoundNumber,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center
                //HeightRequest = 150
            };

            var AnswerImage = new Image
            {
                Source = "answer_now.png"
            };

            listview = new ListView
            {
                ItemsSource = inputs,
                HeightRequest = 30,
                ItemTemplate = new DataTemplate(() =>
                {
                    var textlabel = new Label
                    {
                        TextColor = Color.FromRgb(31, 174, 206)
                    };

                    var cell = new ViewCell
                    {
                        View = new StackLayout
                        {
                            Children =
                            {
                                textlabel
                            }
                        }
                    };

                    textlabel.SetBinding(Label.TextProperty, "Answer");

                    /*
                    TextCell cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, "Answer");
                    */
                    return cell;
                })
                //RowHeight = 150,
            };

            VLayout = new StackLayout
            {
                Children =
                {
                    RoundLabel,
                    AnswerImage,
                    listview
                },
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            listview.ItemSelected += async (sender, args) =>
            {
                ViewController.Invoke(() => listview.IsEnabled = false);
                // GameManager.ViewController.Invoke(() => GameManager.CurrentGame.CurrentVoteChoice = ((PlayerInput)args.SelectedItem).PlayerID);

                var item = args.SelectedItem as PlayerAnswer;
                await GameManager.Vote(item.PlayerId);
                ViewController.Invoke(() => listview.IsEnabled = true);
                ViewController.Invoke(() => ((ListView) sender).SelectedItem = null);
            };

            Display(ViewController);
        }

        public StackLayout VLayout { get; set; }
        public string Vote { get; set; }
        public ListView listview { get; set; }

        internal void Display(ContentPageController ViewController)
        {
            ViewController.Invoke(() => { ViewController.View.Content = VLayout; });
        }
    }
}