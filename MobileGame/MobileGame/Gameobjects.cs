using System;

namespace MobileGame
{
    internal class GameIDObject
    {
        public string GameId { get; set; }
        public int RoundNumber { get; set; }
    }

    // Object types to be returned from server
    internal class GameLobbyResponse : GameIDObject
    {
        public string[] Names { get; set; }
        public bool Admin { get; set; }
    }

    internal class QuestionResponse : GameIDObject
    {
        public string Question { get; set; }
    }

    internal class WaitingResponse : GameIDObject
    {
        public ReadyStatus[] Players { get; set; }
    }

    internal class VoteResponse : GameIDObject
    {
        public PlayerAnswer[] PlayerAnswers { get; set; }
        public string Question { get; set; }
    }

    internal class ResultsResponse : GameIDObject
    {
        public PlayerGame[] PlayerGames { get; set; }
        public PlayerInput[] PlayerInputs { get; set; }
        public bool Admin { get; set; }
    }

    internal class EndOfGameResponse : GameIDObject
    {
        public PlayerGame[] PlayerGames { get; set; }
        public PlayerInput[] PlayerInputs { get; set; }
    }

    public class GetActiveGamesResponse
    {
        public string GameId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AdminName { get; set; }
    }

    internal class DialogueResponse
    {
        public string Dialogue { get; set; }
    }

    // Objects to be sent to server
    internal class VoteRequest : GameIDObject
    {
        public string Vote { get; set; }
    }

    internal class AnswerRequest : GameIDObject
    {
        public string Answer { get; set; }
    }

    //Game Objects
    internal class ReadyStatus
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }

    internal class PlayerAnswer
    {
        public string PlayerId { get; set; }
        public string Answer { get; set; }
    }


    internal class PlayerInput
    {
        public string Answer { get; set; }
        public string PlayerId { get; set; }
        public string Vote { get; set; }
        public int Score { get; set; }
        public int VotesRecieved { get; set; }
        public string Name { get; set; }
    }

    public class PlayerGame
    {
        public string Name { get; set; }
        public string PlayerId { get; set; }
        public int Points { get; set; }
    }
}
