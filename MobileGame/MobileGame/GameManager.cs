using System.Net;
using System.Threading.Tasks;

namespace MobileGame
{
    //Manages the interaction between the player and the server
    //y u so static
    internal static class GameManager
    {
        private static LeeHub _hub;
        public static GameSelectLayout GSLayout { get; set; }

        public static LeeHub Hub
        {
            get
            {
                if (_hub == null || !_hub.IsConnected)
                {
                    _hub = new LeeHub();
                    _hub.SetupGame(CookieJar).Wait();
                }
                return _hub;
            }
        }

        public static CookieContainer CookieJar { get; set; }
        public static ContentPageController ViewController { get; set; }
        public static string GameId { get; set; }
        public static bool AmAdmin { get; set; }


        public static async Task JoinGame(string id)
        {
            GameId = id;
            ViewController.NavDrawer.ShowMenu();
            await Hub.JoinGame(id).ConfigureAwait(false);
        }

        public static void SetupGame(CookieContainer cookieJar, ContentPageController viewController)
        {
            ViewController = viewController;
            CookieJar = cookieJar;
        }

        internal static async Task Vote(string vote)
        {
            await Hub.SendVote(vote, GameId);
        }

        public static async Task StartGame()
        {
            await Hub.StartGame(GameId);
        }

        internal static async Task NewRound()
        {
            await Hub.NewRound(GameId);
        }

        internal static async Task Answer(string text)
        {
            await Hub.SendAnswer(text, GameId).ConfigureAwait(false);
        }

        public static async Task RefreshGame()
        {
            await Hub.JoinGame(GameId);
        }

        internal static async Task CreateGame()
        {
            // must get a gameID from an API, also creates game in database
            var gameId = await WebApiManager.CreateGame(CookieJar).ConfigureAwait(false);
            AmAdmin = true;
            await JoinGame(gameId).ConfigureAwait(false);
        }

        internal static void Close()
        {
            ViewController.NavDrawer.HideMenu();
            CookieJar = null;
            Hub.Close();
        }

        internal static void EndGame()
        {
            GameId = null;
            var GSLayout = new GameSelectLayout(ViewController);
        }

        //views called by the server to be displayed

        internal static void DisplayLobby(GameLobbyResponse GLResponse)
        {
            if (GLResponse.GameId == GameId)
            {
                var GLLayout = new GameLobbyLayout(GLResponse, ViewController);
            }
        }

        internal static void DisplayQuestion(QuestionResponse QResponse)
        {
            if (QResponse.GameId == GameId)
            {
                var QLayout = new QuestionLayout(QResponse, ViewController);
            }
        }

        internal static void DisplayWaiting(WaitingResponse WResponse)
        {
            if (WResponse.GameId == GameId)
            {
                var WLayout = new WaitingLayout(WResponse, ViewController);
            }
        }

        internal static void DisplayVote(VoteResponse VResponse)
        {
            if (VResponse.GameId == GameId)
            {
                var answerLayout = new VoteLayout(VResponse, ViewController);
            }
        }

        internal static void DisplayResults(ResultsResponse RResponse)
        {
            if (RResponse.GameId == GameId)
            {
                var RLayout = new ResultsLayout(RResponse, ViewController);
            }
        }

        internal static void DisplayDialogue(DialogueResponse DiaResponse)
        {
            var DLayout = new DialogueLayout(DiaResponse, ViewController);
        }

        internal static void DisplayEndOfGame(EndOfGameResponse EOGResponse)
        {
            if (EOGResponse.GameId == GameId)
            {
                var EOGLayout = new EndOfGameLayout(EOGResponse, ViewController);
            }
        }

        internal static void ShowException(string message)
        {
            ViewController.Invoke(() => ViewController.DisplayAlert("Error", message, "OK").ConfigureAwait(false));
        }

        // shows list of active games
        internal static void SetGetActiveGames(GetActiveGamesResponse[] games)
        {
            ViewController.Invoke(() => { GSLayout.addActiveGames(games, ViewController); });
        }

        public static async Task GetActiveGames(GameSelectLayout gameselectlayout)
        {
            GSLayout = gameselectlayout;
            await Hub.GetActiveGames();
        }
    }
}