namespace MobileGame
{
    public interface IPushService
    {
        DeviceSettings GetDeviceSettings();
    }

    public class DeviceSettings
    {
        public string Token { get; set; }
        public string DeviceName { get; set; }
    }
}