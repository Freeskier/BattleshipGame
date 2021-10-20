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
  
        public static IRoomManager _roomManager;
        

        public GameHub(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        public async Task SendChallenge(ChallengeUserModel model)
        {
            var userConID = _roomManager.GetUserConnectionID(model.ToUser);
            await Clients.Client(userConID).ChallengeUserCallback(model);
        }

        public async Task AcceptChallenge(ChallengeUserModel model)
        {
            var roomID = _roomManager.CreateRoom(model.FromUser, model.ToUser, out string playerAconnID, out string playerBconnID);
            await Clients.All.StartGame(new StartGameModel {
                RoomID = roomID,
                StartingUserConnID = playerAconnID
            });

            _roomManager.GetMoveResponseData(roomID, Context.ConnectionId, false, out MoveResponseModel respA, out MoveResponseModel respB);
            await Clients.Client(playerBconnID).MoveResponse(respB);
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
            lock(_roomManager)
            {
                _roomManager.AddUserToConnected(Context.ConnectionId, Context.User.Identity.Name);
                Console.WriteLine(Context.User.Identity.Name);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            lock(_roomManager)
            {
                _roomManager.RemoveUserFromConnected(Context.ConnectionId);
                Console.WriteLine("discon " + Context.User.Identity.Name);
            }
            await base.OnDisconnectedAsync(exception);
        }

    }
}