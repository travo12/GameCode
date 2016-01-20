using System;
using MobileGame;
using MobileGame.iOS;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof (LoginPage), typeof (LoginPageRenderer))]

namespace MobileGame.iOS
{
    public class LoginPageRenderer : PageRenderer
    {
        private bool IsShown;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            // Fixed the issue that on iOS 8, the modal wouldn't be popped.
            // url : http://stackoverflow.com/questions/24105390/how-to-login-to-facebook-in-xamarin-forms
            if (!IsShown)
            {
                IsShown = true;

                var auth = new OAuth2Authenticator(Constants.FacebookClientId, // your OAuth2 client id
                    Constants.FaceBookScope,
                    // The scopes for the particular API you're accessing. The format for this will vary by API.
                    new Uri(Constants.FacebookAuthUrl), // the auth URL for the service
                    new Uri(Constants.FacebookRedirect)); // the redirect URL for the service


                auth.Completed += (sender, eventArgs) =>
                {
                    // We presented the UI, so it's up to us to dimiss it on iOS.
                    App.Instance.SuccessfulLoginAction(eventArgs.Account.Properties["access_token"]);

                    if (eventArgs.IsAuthenticated)
                    {
                        // Use eventArgs.Account to do wonderful things
                        //App.Instance.SaveToken(eventArgs.Account.Properties["access_token"]);
                    }
                };

                PresentViewController(auth.GetUI(), true, null);
            }
        }
    }
}