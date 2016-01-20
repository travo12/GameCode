using Xamarin.Forms;

namespace MobileGame
{
    //Shows a screen to display a message from the server, Reverts to previous screen when button pushed
    internal class DialogueLayout
    {
        public DialogueLayout(DialogueResponse DResponse, ContentPageController ViewController)
        {
            var labelLarge = new Label
            {
                Text = "Message From Server",
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 55
            };

            var DialogueLabel = new Label
            {
                Text = DResponse.Dialogue,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 110
            };

            dialoguebutton = new Button
            {
                Text = "Ok",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof (Button)),
                VerticalOptions = LayoutOptions.Fill,
                //TranslationY = 2,
                HeightRequest = 55
                //IsVisible = false,
            };
            // The View
            DiaLayout = new StackLayout
            {
                Children =
                {
                    labelLarge,
                    DialogueLabel,
                    dialoguebutton
                },
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };


            dialoguebutton.Clicked += (sender, args) =>
            {
                ViewController.Invoke(() => dialoguebutton.IsEnabled = false);
                // return to previous sceen
                Display(ViewController, StoredContent);
                ViewController.Invoke(() => dialoguebutton.IsEnabled = false);
            };

            StoredContent = ViewController.View.Content;
            Display(ViewController);
        }

        public StackLayout DiaLayout { get; set; }
        public Button dialoguebutton { get; set; }
        public View StoredContent { get; set; }

        internal void Display(ContentPageController ViewController)
        {
            ViewController.Invoke(() => { ViewController.View.Content = DiaLayout; });
        }

        internal void Display(ContentPageController ViewController, View StoredContent)
        {
            ViewController.Invoke(() => { ViewController.View.Content = StoredContent; });
        }
    }
}