using Microsoft.AspNet.SignalR.Client;
using Xamarin.Forms;

namespace MobileGame
{
    public class PushManager : IPushManager
    {
        #region IPushManager implementation

        public void RegisterDevice(IHubProxy hub)
        {
            var deviceSettings = DependencyService.Get<IPushService>().GetDeviceSettings();
            hub.Invoke("RegisterDevice", deviceSettings);
        }

        #endregion
    }

    public interface IPushManager
    {
        void RegisterDevice(IHubProxy hub);
    }
}