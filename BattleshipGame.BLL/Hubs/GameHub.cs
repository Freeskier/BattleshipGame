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
                        Date = DateTime.Now.ToShortDateString()
                    });
            await Clients.Clients(usrFromID, usrToID).StartGame(new StartGameModel {
                RoomID = roomID,
                StartingUserConnID = usrFromID
            });

            _roomManager.GetMoveResponseData(roomID, Context.ConnectionId, false, out MoveResponseModel respA, out MoveResponseModel respB);
            await Clients.Client(usrToID).MoveResponse(respB);
            await Clients.Client(Context.ConnectionId).MoveResponse(respA);
        }

        public async Task MakeMove(MoveModel model)
        {
            var extraMove = _roomManager.ProcessMove(model, Context.ConnectionId, out string enemyConnID);
            _roomManager.GetMoveResponseData(model.RoomID, Context.ConnectionId, extraMove, out MoveResponseModel respA, out MoveResponseModel respB);
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
            _lobbyManager.LeaveLobby(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

    }
}