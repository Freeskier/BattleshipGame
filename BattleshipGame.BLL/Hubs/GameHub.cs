using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleshipGame.BLL.Game.GameLogic.Interfaces;
using BattleshipGame.BLL.Hubs.Models;
using BattleshipGame.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BattleshipGame.BLL.Hubs
{
    public class GameHub : Hub<IGameHub>
    {
  
        public readonly IRoomManager _roomManager;
        public readonly ILobbyManager _lobbyManager;
        public readonly IHubContext<ChatHub, IChatHub> _chatHub;
        

        public GameHub(IRoomManager roomManager, IHubContext<ChatHub, IChatHub> chatHub, ILobbyManager lobbyManager)
        {
            _roomManager = roomManager;
            _chatHub = chatHub;
            _lobbyManager = lobbyManager;
        }

        public async Task SendChallenge(ChallengeUserModel model)
        {
            var userConID = _lobbyManager.ConnectedUsers[model.ToUser].gameID;
            await Clients.Client(userConID).ChallengeUserCallback(model);
        }

        public async Task AcceptChallenge(ChallengeUserModel model)
        {
            var usrFromID = _lobbyManager.ConnectedUsers[model.FromUser].gameID;
            var usrToID = _lobbyManager.ConnectedUsers[model.ToUser].gameID;
            var roomID = _roomManager.CreateRoom(usrFromID, usrToID, model.FromUser, model.ToUser);

            await _chatHub.Clients.Clients(_lobbyManager.ConnectedUsers[model.FromUser].chatID,
                _lobbyManager.ConnectedUsers[model.ToUser].chatID).ReceiveMessage(
                    new MessageModel {
                        Text = "Created room: " + roomID,
                        User = "SERVER",
                        Date = DateTime.Now.ToString("dd.MM.yyyy, hh:mm:ss")
                    });
            await Clients.Clients(usrFromID, usrToID).StartGame(new StartGameModel {
                RoomID = roomID,
                StartingUserConnID = usrFromID
            });

            _roomManager.GetMoveResponseData(roomID, Context.ConnectionId, false, out MoveResponseModel respA, out MoveResponseModel respB);
            await Clients.Client(usrToID).MoveResponse(respB);
            await Clients.Client(Context.ConnectionId).MoveResponse(respA);
            await _chatHub.Clients.All.LoggedUsers(_lobbyManager.GetLoggedUsers());
        }

        public async Task MakeMove(MoveModel model)
        {
            var extraMove = _roomManager.ProcessMove(model, Context.ConnectionId, out string enemyConnID);

            _roomManager.GetMoveResponseData(model.RoomID, Context.ConnectionId, extraMove, out MoveResponseModel respA, out MoveResponseModel respB);
            
            if(_roomManager.IsGameOver(model.RoomID, out string winningUser))
            {
                await SendGameOver("", enemyConnID, winningUser);
                return;
            }
            await Clients.Client(enemyConnID).MoveResponse(respB);
            await Clients.Client(Context.ConnectionId).MoveResponse(respA);
    
        }


        public override async Task OnConnectedAsync()
        {
            _lobbyManager.JoinLobby(Context.User.Identity.Name, "", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var enemyConnID = _roomManager.GetUserEnemyConnID(Context.ConnectionId);
            if(!enemyConnID.Equals(""))
            {
                var winningUser = _lobbyManager.GetUsername(enemyConnID);
                await SendGameOver("Lost connection", enemyConnID, winningUser);
            }
            _lobbyManager.LeaveLobby(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        private async Task SendGameOver(string serverMessage, string enemyConnID, string winningUser) 
        {
            _roomManager.DeleteRoomWithUser(Context.ConnectionId);
            await Clients.Clients(Context.ConnectionId, enemyConnID).GameOver(new GameOverModel {
                    WinningUser = winningUser,
                    ServerMessage = serverMessage
                });
            await _chatHub.Clients.Clients(_lobbyManager.GetChatConnID(Context.ConnectionId), 
                _lobbyManager.GetChatConnID(enemyConnID)).ReceiveMessage(
                    new MessageModel {
                        Text = $"Game Over! User {winningUser} wins! " + $"\n{serverMessage}",
                        User = "SERVER",
                        Date = DateTime.Now.ToString("dd.MM.yyyy, hh:mm:ss")
                    });
            await _chatHub.Clients.All.LoggedUsers(_lobbyManager.GetLoggedUsers());
        }

    }
}