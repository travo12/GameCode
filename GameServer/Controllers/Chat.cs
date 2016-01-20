using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Planc.Dal;
using Planc.Dal.GameModels.LeeGame;
using System.Linq;

namespace Planc.Controllers
{
    
    public class ChatHub : Hub
    {
        public void SendMessage(string text)
        {
            Clients.All.RecieveMessage(text);
        }
    }
}