using System;
using Foundation;
using MobileGame.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof (PushService))]

namespace MobileGame.iOS
{
    public class PushService : IPushService
    {
        #region IPushService implementation

        public static NSData DeviceToken { get; set; }

        public DeviceSettings GetDeviceSettings()
        {
            Console.Out.WriteLine("Called GetDeviceSettings");
            if (DeviceToken == null)
                throw new NullReferenceException("DeviceToken has not been set;");
            var deviceString = DeviceToken.ToString();
            Console.Out.WriteLine("Devicestring: " + deviceString);
            return new DeviceSettings
            {
                DeviceName = "apns",
                Token = deviceString
            };
        }

        #endregion
    }
}