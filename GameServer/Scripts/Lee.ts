/// <reference path="typings/angularjs/angular.d.ts" />
/// <reference path="typings/signalr/signalr.d.ts" />

interface Window {
    gameId: string;
    PlayerId: string;
}

interface Scope {
    Inputs: PlayerInput[];
    State: number;
    Question: string;
    PlayerId: string;
    Waiting: boolean;
    RefreshTimer: number;
    WinnerText: string;
    VR: string;
    Points: string;
    LeeHub: HubProxy;
    GameID: string;
    Players: PlayerInput[];
    Admin: string;
    PlayerGames: PlayerGame[];
    PlayersReady: ReadyStatus[];
    Refresh(): void;
    updatePlayerName(): void;
    startInterval(): void;
    startGame(): void;
    setPlayerAnswer(): void;
    setPlayerVote(playerID: string): void;
    newRound(): void;
    joinGame(): void;
    sendChat(): void;
    $apply(any): any;
    LobbyData: GameLobbyResponse;
    WaitingPlayers: ReadyStatus[];
    PlayerAnswers: PlayerAnswer[];
    ResultsData: ResultsResponse;
}
interface GameIDObject {
    GameId: string
}
interface GameLobbyResponse extends GameIDObject {
    Names: string[],
    Admin: boolean
}
interface QuestionResponse extends GameIDObject {
    Question: string
}
interface WaitingResponse extends GameIDObject {
    Players: ReadyStatus[]
}
interface VoteResponse extends GameIDObject {
    PlayerAnswers: PlayerAnswer[]
}
interface PlayerAnswer {
    PlayerId: string,
    Answer: string
}
interface ResultsResponse {
    PlayerGames: PlayerGame[],
    PlayerInputs: PlayerInput[],
    RoundNumber: number,
    Admin: boolean
}

interface Game {
    State: number;
    Rounds: Round[];
    CurrentRoundNum: number;

}
interface Round {
    PlayerInputs: PlayerInput[];
    Question: string;
}
interface ReadyStatus {
    Name: string;
    Status: string;
}
interface PlayerInput {
    Answer: string;
    PlayerId: string;
    Vote: string;
    Score: number;
    VotesRecieved: number;
    Name: string;
}
interface PlayerGame {
    Name: string;
    PlayerId: string;
    Points: number;
}
interface SignalR {
    leeHub: HubProxy;
}
interface HubProxy {
    server: Server;
    client: Client;
}

interface Client {
    setPlayers(players: PlayerInput[], gameID: string);
    // updateGameState(state: number);
    //playerAnswered(response: AnswerResponse);
    playerVoted(playerName: string, gameID: string);
    displayQuestion(questionResponse: QuestionResponse);
    setVote(inputs: PlayerInput[], gameID: string);
    setResults(inputs: PlayerInput[], PlayerGames: PlayerGame[], gameID: string);
    setEnd(gameID: string);
    setAdmin(admin: string, gameID: string);
    setPlayerReadyStatus(readyStatus: ReadyStatus[], gameID: string);
    chatUpdate(name: string, message: string, gameID: string);
    displayLobby(lobbyData: GameLobbyResponse);
    displayWaiting(waitingData: WaitingResponse);
    displayVote(voteData: VoteResponse);
    displayResults(resultData: ResultsResponse);
}
interface Server {
    joinGame(gameId: string): JQueryPromise<string>;
    answer(data: AnswerRequest): JQueryPromise<string>;
    newRound(gameId: string): JQueryPromise<string>;
    start(gameId: string): JQueryPromise<string>;
    vote(data: VoteRequest): JQueryPromise<string>;
    sendChatMessage(data: MessageRequest): JQueryPromise<string>;
}

interface AnswerRequest {
    Answer: string;
    GameId: string;
}

interface VoteRequest {
    Vote: string;
    GameId: string;
}
interface MessageRequest {
    Message: string;
    GameId: string;
}


var lee = angular.module("lee", []).filter("vote", function () {
    return function (voteInput: any[], answerInput: PlayerInput) {
        var returnArray = voteInput.filter(function (elem) {
            return elem.Vote === answerInput.PlayerId;
        });
        return returnArray;
    };
});

lee.controller("LeeController", [
    "$scope", "$http", function ($scope: Scope, $http) {
        $scope.State = 1;

        $scope.PlayerId = window.PlayerId;
        $scope.GameID = window.gameId;
        $scope.Waiting = false;
        $scope.WinnerText = "";
        //Orderby Predicates
        $scope.VR = "-VotesRecieved";
        $scope.Points = "-Points";
        $scope.LeeHub = $.connection.leeHub;


        $("#menu-toggle").click(function (e) {
            e.preventDefault();
            $("#wrapper").toggleClass("toggled");
        });

        function winnertext(playerInputs:PlayerInput[]) {
            var max = playerInputs.sort(function (valA, valB) {
                return valB.Score - valA.Score;
            })[0].Score;
            var names = [];
            playerInputs.forEach(function (pinput) {
                if (pinput.Score === max) {
                    names.push(pinput.Name);
                }
            });
            var end = names.length > 1 ? " Tied!" : " Won";
            return names.join(" and ") + end;
        }


        $scope.startGame = function () {
            $scope.LeeHub.server.start(window.gameId);
        };

        $scope.setPlayerAnswer = function () {
            var answer = $("#answer").val();
            $scope.LeeHub.server.answer({ Answer: answer, GameId: window.gameId });

            $("#answer").val("");
        };

        $scope.setPlayerVote = function (playerId) {
            $scope.LeeHub.server.vote({ Vote: playerId, GameId: window.gameId }).fail(function () {
                alert("Cannot vote for yourself");

            }).done(function () {
                $scope.Waiting = true
            });
        };

        $scope.sendChat = function () {
            var message = $(".inputmessage").val();
            $scope.LeeHub.server.sendChatMessage({ Message: message, GameId: window.gameId });
            $(".inputmessage").val("");
        }
        $scope.newRound = () => {
            $scope.LeeHub.server.newRound(window.gameId);
        };
        $scope.joinGame = function () {
            $.connection.hub.start().done(function () {
                $scope.LeeHub.server.joinGame($scope.GameID);
            });
        }
        $scope.joinGame();

        //$scope.LeeHub.client.setPlayers = function (players: PlayerInput[], gameID: string) {
        //    $scope.$apply(function () {
        //        $scope.Players = players;
        //    });
        //}
        $scope.LeeHub.client.displayLobby = function (lobbyData: GameLobbyResponse) {
            $scope.$apply(function () {
                $scope.State = 1;
                $scope.Waiting = false;
                $scope.LobbyData = lobbyData;
            });
        }
        $scope.LeeHub.client.displayQuestion = function (questionData: QuestionResponse) {
            $scope.$apply(function () {
                $scope.Question = questionData.Question;
                $scope.State = 2;
                $scope.Waiting = false;
            });
        }
        $scope.LeeHub.client.displayWaiting = function (waitingData: WaitingResponse) {
            $scope.$apply(function () {
                $scope.WaitingPlayers = waitingData.Players;
                $scope.Waiting = true;
            });
        }
        $scope.LeeHub.client.displayVote = function (voteData: VoteResponse) {
            $scope.$apply(function () {
                $scope.PlayerAnswers = voteData.PlayerAnswers;
                $scope.State = 3;
                $scope.Waiting = false;
            });
        }
        $scope.LeeHub.client.displayResults = function (resultsData: ResultsResponse) {
            $scope.$apply(function () {
                $scope.ResultsData = resultsData;
                $scope.State = 4;
                $scope.Waiting = false;
                $scope.WinnerText = winnertext(resultsData.PlayerInputs);
                $scope.PlayersReady = null;
            });
        }
        $scope.LeeHub.client.setEnd = function (gameID: string) {
            $scope.$apply(function () {
                $scope.State = 5;
                $scope.Waiting = false;
            });
        }
        $scope.LeeHub.client.setAdmin = function (admin: string, gameID: string) {
            $scope.$apply(function () {
                $scope.Admin = admin;
            });
        }
        $scope.LeeHub.client.setPlayerReadyStatus = function (readyStatus: ReadyStatus[], gameID: string) {
            $scope.$apply(function () {
                $scope.PlayersReady = readyStatus;
            });
        }
        $scope.LeeHub.client.chatUpdate = function (name: string, message: string, gameID: string) {
            var line = "<li>" + name + ": " + message + "</li>";
            $(".messages").append(line);
        }

    }
]).directive("ngEnter", function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});;