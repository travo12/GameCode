using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace MobileGame.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // Process any potential notification data from launch
            //ProcessNotification (options);

            // Register for Notifications
            var settings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert
                | UIUserNotificationType.Badge
                | UIUserNotificationType.Sound,
                null); //satish: changed from "new NSSet()" to null 
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            // ...
            // Your other code here
            // ...
            Forms.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }

        public override void DidRegisterUserNotificationSettings(UIApplication application,
            UIUserNotificationSettings notificationSettings)
        {
            // without this RegisteredForRemoteNotifications doesn't fire on iOS 8!
            //application.RegisterForRemoteNotifications();
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Console.Out.WriteLine("Starting Registering");
            // Connection string from your azure dashboard
//			var cs = SBConnectionString.CreateListenAccess(
//				new NSUrl("sb://leegame.servicebus.windows.net/"),
//				"CuzYsN06YZEVOV5GqaNA8G1rnFT3QgvKOPGbT3CCiM8=");
//			//Endpoint=sb://leegame.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=CuzYsN06YZEVOV5GqaNA8G1rnFT3QgvKOPGbT3CCiM8=
//			// Register our info with Azure
//			var hub = new SBNotificationHub (cs, "leepush");
//			hub.RegisterNativeAsync (deviceToken, null, err => {
//				if (err != null)
//					Console.WriteLine("Error: " + err.Description);
//				else
//					Console.WriteLine("Success");
//			});
            PushService.DeviceToken = deviceToken;
        }

        public override void ReceivedRemoteNotification(UIApplication app, NSDictionary userInfo)
        {
            // Process a notification received while the app was already open
            // ProcessNotification (userInfo);
            Console.Out.WriteLine("Push Notification Recieved");
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }
    }
}