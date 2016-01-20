using Microsoft.AspNet.SignalR.Client;

namespace MobileGame
{
    internal class SignalRConn
    {
        public SignalRConn()
        {
            hubConnection = new HubConnection("https://games.limeyjohnson.com/signalr/hubs");
            gameProxy = hubConnection.CreateHubProxy("GameHub");

            //gameProxy.On<game>

            hubConnection.Start();
        }

        public HubConnection hubConnection { get; set; }
        public IHubProxy gameProxy { get; set; }
    }
}