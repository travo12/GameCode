namespace MobileGame
{
    public static class Constants
    {
        static Constants()
        {
            PushManager = new PushManager();
        }

#if DEBUG
        public static readonly string EndPoint = "https://craggtrav.azurewebsites.net";
#else
        public static readonly string EndPoint = "https://unsafespacesgame.com";
#endif
        public static readonly string LeeHubEndPoint = EndPoint + "/signalr/hubs";
        public static readonly string LeeHubName = "LeeHub";
        public static readonly string LoginEndPoint = EndPoint + "/Account/Login";
        public static readonly string FaceBookScope = "user_friends,email,public_profile";
        public static readonly string FaceBookLoginEndpoint = EndPoint + "/Account/MobileExternalLoginConfirmation";
        public static readonly string CreateGameEndPoint = EndPoint + "/api/Mobile";
        public static IPushManager PushManager { get; private set; }
        public static readonly string FacebookClientId = "933974503302385";
        public static readonly string FacebookRedirect = EndPoint + "/signin-facebook";
        public static readonly string FacebookAuthUrl = "https://m.facebook.com/dialog/oauth/";
    }
}