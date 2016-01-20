using System.Text;
using Android.App;
using Android.Content;
using Android.Util;
using Gcm.Client;

[assembly: Permission(Name = "com.planc.plancgames.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.planc.plancgames.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is needed only for Android versions 4.0.3 and below

[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace MobileGame.Droid
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new[] {Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE},
        Categories = new[] {"com.planc.plancgames"})]
    [IntentFilter(new[] {Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK},
        Categories = new[] {"com.planc.plancgames"})]
    [IntentFilter(new[] {Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY},
        Categories = new[] {"com.planc.plancgames"})]
    public class MyBroadcastReceiver : GcmBroadcastReceiverBase<PushHandlerService>
    {
        public const string TAG = "MyBroadcastReceiver-GCM";
        public static string[] SENDER_IDS = {MainActivity.SenderID};
    }


    [Service] // Must use the service tag
    public class PushHandlerService : GcmServiceBase
    {
        public PushHandlerService() : base(MainActivity.SenderID)
        {
            Log.Info(MyBroadcastReceiver.TAG, "PushHandlerService() constructor");
        }

        public static string RegistrationID { get; private set; }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM Registered: " + registrationId);
            RegistrationID = registrationId;

            //createNotification("PushHandlerService-GCM Registered...",
            //	"The device has been Registered!");

            PushService.GCMToken = registrationId;
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info(MyBroadcastReceiver.TAG, "GCM Message Received!");

            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key));
            }

            var messageText = intent.Extras.GetString("message");
            if (!string.IsNullOrEmpty(messageText))
            {
                createNotification("New hub message!", messageText);
            }
            else
            {
                createNotification("Unknown message details", msg.ToString());
            }
        }


        private void createNotification(string title, string desc)
        {
//			//Create notification
//			var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
//
//			//Create an intent to show UI
//			var uiIntent = new Intent(this, typeof(MainActivity));
//
//			//Create the notification
//			var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);
//
//			//Auto-cancel will remove the notification once the user touches it
//			notification.Flags = NotificationFlags.AutoCancel;
//
//			//Set the notification info
//			//we use the pending intent, passing our ui intent over, which will get called
//			//when the notification is tapped.
//			notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));
//
//			//Show the notification
//			notificationManager.Notify(1, notification);
            dialogNotify(title, desc);
        }

        protected void dialogNotify(string title, string message)
        {
            MainActivity.instance.RunOnUiThread(() =>
            {
                var dlg = new AlertDialog.Builder(MainActivity.instance);
                var alert = dlg.Create();
                alert.SetTitle(title);
                alert.SetButton("Ok", delegate { alert.Dismiss(); });
                alert.SetMessage(message);
                alert.Show();
            });
        }


        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM Unregistered: " + registrationId);

            createNotification("GCM Unregistered...", "The device has been unregistered!");
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Log.Warn(MyBroadcastReceiver.TAG, "Recoverable Error: " + errorId);

            return base.OnRecoverableError(context, errorId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error(MyBroadcastReceiver.TAG, "GCM Error: " + errorId);
        }
    }
}