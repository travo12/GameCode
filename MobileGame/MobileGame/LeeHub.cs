using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace MobileGame
{
    //SignalR Hub used to communicate with server
    internal class LeeHub
    {
        public HubConnection hubConnection { get; set; }
        public IHubProxy GameProxy { get; set; }

        public bool IsConnected
        {
            get { return hubConnection.State == ConnectionState.Connected; }
        }

        public async Task SetupGame(CookieContainer cookiejar)
        {
            hubConnection = new HubConnection(Constants.LeeHubEndPoint);
            hubConnection.CookieContainer = cookiejar;

            GameProxy = hubConnection.CreateHubProxy(Constants.LeeHubName);

            try
            {
                await hubConnection.Start().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception from Hub: " + e.Message);
            }

            hubConnection.ConnectionSlow += () => Debug.WriteLine("Connection is slow!");
            hubConnection.Error += (Exception e) => { Debug.WriteLine("Exception occured!" + e.Message); };
            hubConnection.Reconnecting += () => Debug.WriteLine("Reconnecting!");
            hubConnection.Received += (string s) => Debug.WriteLine("Recieved Something! " + s);
            hubConnection.StateChanged +=
                (StateChange s) => Debug.WriteLine("State changed, idk what it means" + s.NewState);


            //Register the user for Push Notifications
            Constants.PushManager.RegisterDevice(GameProxy);


            //functions to be called by Server

            GameProxy.On<GetActiveGamesResponse[]>("GetActiveGames", GameManager.SetGetActiveGames);

            GameProxy.On<GameLobbyResponse>("DisplayLobby", GameManager.DisplayLobby);

            GameProxy.On<QuestionResponse>("DisplayQuestion", GameManager.DisplayQuestion);

            GameProxy.On<WaitingResponse>("DisplayWaiting", GameManager.DisplayWaiting);

            GameProxy.On<VoteResponse>("DisplayVote", GameManager.DisplayVote);

            GameProxy.On<ResultsResponse>("DisplayResults", GameManager.DisplayResults);

            GameProxy.On<EndOfGameResponse>("DisplayEndOfGame", GameManager.DisplayEndOfGame);

            GameProxy.On<DialogueResponse>("DisplayDialogue", GameManager.DisplayDialogue);

            GameProxy.On<string>("ExceptionHandler", GameManager.ShowException);
        }
        // functions to call on server
        public async Task JoinGame(string gameId)
        {
            await GameProxy.Invoke("JoinGame", gameId).ConfigureAwait(false);
        }

        public async Task SendAnswer(string text, string gameId)
        {
            var myrequest = new AnswerRequest {Answer = text, GameId = gameId};
            await GameProxy.Invoke("Answer", myrequest).ConfigureAwait(false);
        }

        public async Task StartGame(string gameId)
        {
            await GameProxy.Invoke("Start", gameId).ConfigureAwait(false);
        }

        public async Task SendVote(string playervote, string gameId)
        {
            var myrequest = new VoteRequest {Vote = playervote, GameId = gameId};
            await GameProxy.Invoke("Vote", myrequest).ConfigureAwait(false);
        }

        //remove all data about current game in preparation for a switch to a new game.
        public async Task RefreshGame(string gameId)
        {
            await GameProxy.Invoke("RefreshGame", gameId).ConfigureAwait(false);
        }

        public async Task NewRound(string gameId)
        {
            await GameProxy.Invoke("NewRound", gameId).ConfigureAwait(false);
        }

        public void Close()
        {
            //things that need to get booted: 
            // i dont think this matters at all
            hubConnection.Stop();
        }

        public async Task GetActiveGames()
        {
            await GameProxy.Invoke("GetActiveGames").ConfigureAwait(false);
        }
    }
}