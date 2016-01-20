/// <reference path="typings/angularjs/angular.d.ts" />
/// <reference path="typings/signalr/signalr.d.ts" />
var lee = angular.module("lee", []).filter("vote", function () {
    return function (voteInput, answerInput) {
        var returnArray = voteInput.filter(function (elem) {
            return elem.Vote === answerInput.PlayerId;
        });
        return returnArray;
    };
});
lee.controller("LeeController", [
    "$scope", "$http", function ($scope, $http) {
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
        function winnertext(playerInputs) {
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
                $scope.Waiting = true;
            });
        };
        $scope.sendChat = function () {
            var message = $(".inputmessage").val();
            $scope.LeeHub.server.sendChatMessage({ Message: message, GameId: window.gameId });
            $(".inputmessage").val("");
        };
        $scope.newRound = function () {
            $scope.LeeHub.server.newRound(window.gameId);
        };
        $scope.joinGame = function () {
            $.connection.hub.start().done(function () {
                $scope.LeeHub.server.joinGame($scope.GameID);
            });
        };
        $scope.joinGame();
        //$scope.LeeHub.client.setPlayers = function (players: PlayerInput[], gameID: string) {
        //    $scope.$apply(function () {
        //        $scope.Players = players;
        //    });
        //}
        $scope.LeeHub.client.displayLobby = function (lobbyData) {
            $scope.$apply(function () {
                $scope.State = 1;
                $scope.Waiting = false;
                $scope.LobbyData = lobbyData;
            });
        };
        $scope.LeeHub.client.displayQuestion = function (questionData) {
            $scope.$apply(function () {
                $scope.Question = questionData.Question;
                $scope.State = 2;
                $scope.Waiting = false;
            });
        };
        $scope.LeeHub.client.displayWaiting = function (waitingData) {
            $scope.$apply(function () {
                $scope.WaitingPlayers = waitingData.Players;
                $scope.Waiting = true;
            });
        };
        $scope.LeeHub.client.displayVote = function (voteData) {
            $scope.$apply(function () {
                $scope.PlayerAnswers = voteData.PlayerAnswers;
                $scope.State = 3;
                $scope.Waiting = false;
            });
        };
        $scope.LeeHub.client.displayResults = function (resultsData) {
            $scope.$apply(function () {
                $scope.ResultsData = resultsData;
                $scope.State = 4;
                $scope.Waiting = false;
                $scope.WinnerText = winnertext(resultsData.PlayerInputs);
                $scope.PlayersReady = null;
            });
        };
        $scope.LeeHub.client.setEnd = function (gameID) {
            $scope.$apply(function () {
                $scope.State = 5;
                $scope.Waiting = false;
            });
        };
        $scope.LeeHub.client.setAdmin = function (admin, gameID) {
            $scope.$apply(function () {
                $scope.Admin = admin;
            });
        };
        $scope.LeeHub.client.setPlayerReadyStatus = function (readyStatus, gameID) {
            $scope.$apply(function () {
                $scope.PlayersReady = readyStatus;
            });
        };
        $scope.LeeHub.client.chatUpdate = function (name, message, gameID) {
            var line = "<li>" + name + ": " + message + "</li>";
            $(".messages").append(line);
        };
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
});
;
//# sourceMappingURL=Lee.js.map