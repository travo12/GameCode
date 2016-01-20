using System;
using Android.Util;
using MobileGame.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof (PushService))]

namespace MobileGame.Droid
{
    public class PushService : IPushService
    {
        internal static string GCMToken { get; set; }

        #region IPushService implementation

        public DeviceSettings GetDeviceSettings()
        {
            Log.Debug("GetDeviceSettings", "Get Device Settings Called on Andriod");
            if (string.IsNullOrEmpty(GCMToken))
                throw new ArgumentNullException("GCMToken is NULL");
            return new DeviceSettings
            {
                Token = GCMToken,
                DeviceName = "gcm"
            };
        }

        #endregion
    }
}