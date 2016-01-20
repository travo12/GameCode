using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Gcm.Client;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
//using Android.Gms.Common;
//using Android.Gms.Gcm;


namespace MobileGame.Droid
{
    [Activity(Label = "Unsafe Spaces", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        public static MainActivity instance;
        public static readonly string SenderID = "740824773325";

        protected override void OnCreate(Bundle bundle)
        {
            instance = this;

            base.OnCreate(bundle);

            //			var intent = new Intent (this, typeof(RegistrationIntentService));
            //			StartService (intent);
            //			GmsClient.
            //			int resultCode = GooglePlayServicesUtil.IsGooglePlayServicesAvailable (this);
            //			if (resultCode != ConnectionResult.Success)
            //			{
            //				if (GooglePlayServicesUtil.IsUserRecoverableError (resultCode))
            //					Log.Debug("PlayServiceCheck", GooglePlayServicesUtil.GetErrorString (resultCode));
            //				else
            //				{
            //					Log.Debug("PlayServiceCheck","Sorry, this device is not supported");
            //					Finish ();
            //				}
            //		   
            //			}
            //			else
            //			{
            //							Log.Debug("PlayServiceCheck","Google Play Services is available.");
            //
            //			}

            //adding to try and get rid of xamarin icon
            ActionBar.SetIcon(Android.Resource.Color.Transparent);

            Forms.Init(this, bundle);
            RegisterWithGCM();
            try
            {
                LoadApplication(new App());
            }
            catch (Exception e)
            {
                Log.Debug("TravTag", "exception has been thrown" + e.Message);
            }
        }

        private void RegisterWithGCM()
        {
            // Check to ensure everything's set up right
            //GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            Log.Info("MainActivity", "Registering...");
            GcmClient.Register(this, SenderID);
        }
    }
}