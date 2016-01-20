using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Azure.NotificationHubs;
using Planc.Dal;
using Planc.Dal.GameModels.LeeGame;
using Planc.Push;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planc.Controllers
{
    [Authorize]
    public class LeeHub : Hub
    {
        private static readonly string UserPrefix = "USER";
        public void GetActiveGames()
        {
            Trace.TraceInformation("GetActiveGames called");
            var playerId = Context.User.Identity.GetUserId();
            var gamelist = GameConstants.Dal.GetWaitingGames(playerId);
            var responses = gamelist.Select(g => new GetActiveGamesResponse { AdminName = g.AdminName, CreatedDate = g.CreatedDate, GameId = g.Id });
            int current = 0;
            var limitetResponses = responses.TakeWhile(g => current++ < 10);
            //return the query to the one who asked
            Clients.Caller.GetActiveGames(limitetResponses);
            Trace.TraceInformation("GetActiveGames Returned");
        }
        public void JoinGame(string gameID)
        {
            Trace.TraceInformation("JoinGame called " + gameID);
            Lee game;
            string playerId;

            game = GameConstants.Dal.LoadGame<Lee>(gameID) as Lee;
            
            if (game == null)
            {
                SendMessage("Game could not be found");

                return;
            }
            
            playerId = Context.User.Identity.GetUserId();

            

            //Check if the player has already been added to the game
              if (!game.PlayerGames.Any(p => p.PlayerId == playerId))
            {
                //Set the admin if this is the first person
                if (!game.PlayerGames.Any())
                {
                    game.Admin = playerId;
                    game.AdminName = Context.User.Identity.Name;
                }
                var playerGame = new PlayerGame()
                {
                    PlayerId = playerId,
                    Points = 0,
                    Name = Context.User.Identity.Name
                };
                game.PlayerGames.Add(playerGame);

                var playerIds = game.PlayerGames.Select(pg => pg.PlayerId);
                var players = GameConstants.Dal.GetPlayersFromIds(playerIds);
                GameConstants.Dal.SaveGame(game);
            }
            Groups.Add(Context.ConnectionId, game.Id).Wait();
            Groups.Add(Context.ConnectionId, UserPrefix + playerId).Wait();
            if (game.State == LeeState.WaitingForPlayers) UpdateClients(game);
            else UpdateClients(game, Clients.Caller);
        }
        public override Task OnReconnected()
        {
            Debug.WriteLine("Client has been reconnected");
            return base.OnReconnected();
        }

        public void Start(string id)
        {
            Trace.TraceInformation("Start called " + id);
            var playerID = Context.User.Identity.GetUserId();
            var game = GameConstants.Dal.LoadGame<Lee>(id) as Lee;
            if (playerID == game.Admin)
            {
                try
                {
                    game.Start();
                }
                catch (LeeException e)
                {
                    SendMessage(e.Message);
                }
                GameConstants.Dal.SaveGame(game);
                UpdateClients(game);
                //ServerConstants.PushManager.SendQuestion(game.Id).Wait();
            }
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Trace.TraceWarning("Client Disconnected");
            Debug.WriteLine("Client has been disconnected");
            return base.OnDisconnected(stopCalled);
        }
        /// <summary>
        /// A Player has answered.
        /// The result of this method is to show the waiting screen to everyone who has already answered and then when everyone has answered to show the voting screen
        /// </summary>
        public void Answer(AnswerRequest data)
        {
            Trace.TraceInformation("Answer called " + data.GameId);
            var playerId = Context.User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(playerId) && !string.IsNullOrEmpty(data.Answer))
            {
                var game = GameConstants.Dal.LoadGame<Lee>(data.GameId) as Lee;
                game.RecordPlayerAnswer(playerId, data.Answer);
                GameConstants.Dal.SaveGame(game);

                //find all of the users that we should update
                var playersToUpdate = game.CurrentRound.PlayerInputs.Where(pi => !string.IsNullOrEmpty(pi.Answer)).Select(pi => pi.PlayerId).ToArray();
                UpdateClients(game, GetUserGroup(playersToUpdate));
            }
        }
        public void Vote(VoteRequest data)
        {
            Trace.TraceInformation("Vote called" + data.GameId);
            var playerId = Context.User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(playerId) || !string.IsNullOrEmpty(data.Vote))
            {
                try
                {
                    var game = GameConstants.Dal.LoadGame<Lee>(data.GameId) as Lee;
                    if (game != null) game.RecordPlayerVote(playerId, data.Vote);
                    GameConstants.Dal.SaveGame(game);
                    var playersToUpdate = game.CurrentRound.PlayerInputs.Where(pi => !string.IsNullOrEmpty(pi.Vote)).Select(pi => pi.PlayerId).ToArray();
                    UpdateClients(game, GetUserGroup(playersToUpdate));
                }
                catch (LeeException ex)
                {
                    SendMessage(ex.Message);
                }
            }
        }
        public void NewRound(string id)
        {
            Trace.TraceInformation("NewRound " + id);
            var game = GameConstants.Dal.LoadGame<Lee>(id) as Lee;
            if (game != null && game.Admin == Context.User.Identity.GetUserId())
            {
                game.NewRound();
                GameConstants.Dal.SaveGame(game);
                UpdateClients(game);
                //ServerConstants.PushManager.SendNewRound(game.Id).Wait();
            }
        }
        public void SendMessage(string message)
        {
            var response = new DialogueResponse {Dialogue = message };
            Clients.Caller.DisplayDialogue(response);
        }
        public void RegisterDevice(PushManager.DeviceRegistration deviceRegistration)
        {
            Trace.TraceInformation("RegisterDevice called ");
            //ServerConstants.PushManager.RegisterDevice(deviceRegistration, Context.User.Identity.GetUserId()).Wait();
        }
        #region Private Helpers
        private void UpdateClients(Lee game, dynamic audience = null)
        {
            audience = audience ?? Clients.Group(game.Id);
            var playerId = Context.User.Identity.GetUserId();
            switch (game.State)
            {
                case LeeState.WaitingForPlayers:
                    //TODO: Determin who the admin is instead of sending true
                    var lobbyResponse = new GameLobbyResponse { GameId = game.Id, Names = game.PlayerGames.Select(pg => pg.Name).ToArray(), RoundNumber = game.CurrentRoundNum };
                    foreach (var player in game.PlayerGames)
                    {
                        lobbyResponse.Admin = player.PlayerId == game.Admin;
                        Clients.Group(UserPrefix + player.PlayerId).DisplayLobby(lobbyResponse);
                    }
                    break;
                case LeeState.QuestionAndAnswer:
                    var playerInputAnswer = game.CurrentRound.PlayerInputs.FirstOrDefault(pi => pi.PlayerId == playerId);
                    if (string.IsNullOrEmpty(playerInputAnswer.Answer))
                    {
                        var questionResponse = new QuestionResponse { Question = game.CurrentRound.Question, GameId = game.Id, RoundNumber = game.CurrentRoundNum };
                        audience.DisplayQuestion(questionResponse);
                    }
                    else
                    {
                        var playersReady = game.CurrentRound.PlayerInputs.Select(p => new ReadyStatus { Name = p.Name, Status = string.IsNullOrEmpty(p.Answer) ? "Not Answered" : "Answered" }).ToArray(); ;
                        audience.DisplayWaiting(new WaitingResponse { Players = playersReady, GameId = game.Id, RoundNumber = game.CurrentRoundNum });
                    }
                    break;
                case LeeState.Vote:
                    var playerInputVote = game.CurrentRound.PlayerInputs.FirstOrDefault(pi => pi.PlayerId == playerId);
                    if (string.IsNullOrEmpty(playerInputVote.Vote))
                    {
                        var answers = game.CurrentRound.PlayerInputsShuffled.Select(pi => new PlayerAnswer { PlayerId = pi.PlayerId, Answer = pi.Answer }).ToArray();
                        audience.DisplayVote(new VoteResponse { PlayerAnswers = answers, GameId = game.Id, RoundNumber = game.CurrentRoundNum, Question = game.CurrentRound.Question });
                    }
                    else
                    {
                        var playersReady = game.CurrentRound.PlayerInputs.Select(p => new ReadyStatus { Name = p.Name, Status = string.IsNullOrEmpty(p.Vote) ? "Not Voted" : "Voted" }).ToArray();
                        audience.DisplayWaiting(new WaitingResponse { Players = playersReady, GameId = game.Id, RoundNumber = game.CurrentRoundNum });
                    }
                    break;
                case LeeState.Results:
                case LeeState.End:
                    var voteResponse = new ResultsResponse
                    {
                        PlayerGames = game.PlayerGames.ToArray(),
                        GameId = game.Id,
                        PlayerInputs = game.CurrentRound.PlayerInputs.ToArray(),
                        RoundNumber = game.CurrentRoundNum
                    };
                    foreach (var resultPlayer in game.PlayerGames)
                    {
                        voteResponse.Admin = resultPlayer.PlayerId == game.Admin;
                        if (game.State == LeeState.Results) Clients.Group(UserPrefix + resultPlayer.PlayerId).DisplayResults(voteResponse);
                        if (game.State == LeeState.End) audience.DisplayEndOfGame(voteResponse);
                    }

                    break;

                default: break;
            }

        }
        private dynamic GetUserGroup(params string[] userIds)
        {
            var ids = userIds.Select(s => UserPrefix + s).ToList();
            return Clients.Groups(ids);
        }

        #endregion
        #region SubClasses
        public class GameIDObject
        {
            public string GameId { get; set; }
            public int RoundNumber { get; set; }
        }
        class GameLobbyResponse : GameIDObject
        {
            public string[] Names { get; set; }
            public bool Admin { get; set; }
        }
        class QuestionResponse : GameIDObject
        {
            public string Question { get; set; }
        }
        class WaitingResponse : GameIDObject
        {
            public ReadyStatus[] Players { get; set; }
        }
        class VoteResponse : GameIDObject
        {
            public string Question { get; set; }
            public PlayerAnswer[] PlayerAnswers { get; set; }
        }
        class ResultsResponse : GameIDObject
        {
            public PlayerGame[] PlayerGames { get; set; }
            public LeePlayerInput[] PlayerInputs { get; set; }
            public bool Admin { get; set; }
        }
        class DialogueResponse 
        {
            public string Dialogue { get; set; }
        }
        public class VoteRequest : GameIDObject
        {
            public string Vote { get; set; }
        }
        public class AnswerRequest : GameIDObject
        {
            public string Answer { get; set; }
        }
        class ReadyStatus
        {
            public string Name { get; set; }
            public string Status { get; set; }
        }
        class PlayerAnswer
        {
            public string PlayerId { get; set; }
            public string Answer { get; set; }
        }
        class GetActiveGamesResponse
        {
            public string AdminName { get; set; }
            public DateTime CreatedDate { get; set; }
            public string GameId { get; set; }
        }
        #endregion
    }



    public class MyHubPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext,
                                                IHubIncomingInvokerContext invokerContext)
        {
            Trace.Fail("HUB EXCEPTION: " + exceptionContext.Error.Message);
            dynamic caller = invokerContext.Hub.Clients.Caller;
            caller.ExceptionHandler(exceptionContext.Error.Message);
        }
    }
}