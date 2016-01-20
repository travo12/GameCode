using System;
using Android.App;
using MobileGame;
using MobileGame.Driod;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof (LoginPage), typeof (LoginPageRenderer))]

namespace MobileGame.Driod
{
    public class LoginPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            // this is a ViewGroup - so should be able to load an AXML file and FindView<>
            var activity = Context as Activity;

            var auth = new OAuth2Authenticator(Constants.FacebookClientId, // your OAuth2 client id
                Constants.FaceBookScope,
                // The scopes for the particular API you're accessing. The format for this will vary by API.
                new Uri(Constants.FacebookAuthUrl), // the auth URL for the service
                new Uri(Constants.FacebookRedirect)); // the redirect URL for the service

            auth.Completed += (sender, eventArgs) =>
            {
                App.Instance.SuccessfulLoginAction(eventArgs.Account.Properties["access_token"]);
                if (eventArgs.IsAuthenticated)
                {
                    //App.Instance.SuccessfulLoginAction(eventArgs.Account.Properties["access_token"]);
                    // Use eventArgs.Account to do wonderful things
                    //App.Instance.SaveToken(eventArgs.Account.Properties["access_token"]);
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }
    }
}