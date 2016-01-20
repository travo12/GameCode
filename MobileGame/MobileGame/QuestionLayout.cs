using Xamarin.Forms;

namespace MobileGame
{
    internal class QuestionLayout
    {
        //Shows Question Page
        public QuestionLayout(QuestionResponse QResponse, ContentPageController ViewController)
        {
            var question = QResponse.Question;
            var RoundNumber = QResponse.RoundNumber;

            var RoundLabel = new Label
            {
                Text = "Round " + RoundNumber,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 150
            };
            var questionlabel = new Label
            {
                Text = question,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            QuestionAnswer = new Entry
            {
                Placeholder = "Answer Here",
                VerticalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Text
            };
            button = new Button
            {
                Text = "Submit!",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof (Button)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill
            };
            QLayout = new StackLayout
            {
                Children =
                {
                    RoundLabel,
                    questionlabel,
                    QuestionAnswer,
                    button
                },
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            button.Clicked += async (sender, args) =>
            {
                ViewController.Invoke(() => button.IsEnabled = false);
                if (QuestionAnswer.Text != null)
                {
                    await GameManager.Answer(QuestionAnswer.Text);
                }
                else
                {
                    ViewController.Invoke(() =>
                        ViewController.DisplayAlert("Nothing in Answer", "Don't forget to answer before submitting.",
                            "OK").ConfigureAwait(false));
                }
                ViewController.Invoke(() => button.IsEnabled = true);
            };

            Display(ViewController);
        }

        public StackLayout QLayout { get; set; }
        public Button button { get; set; }
        public Entry QuestionAnswer { get; set; }
        public string Answer { get; set; }

        internal void Display(ContentPageController ViewController)
        {
            ViewController.Invoke(() => { ViewController.View.Content = QLayout; });
        }
    }
}