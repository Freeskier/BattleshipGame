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
    [Authorize]
    public class GameHub : Hub<IGameHub>
    {
  
        public static IRoomManager _roomManager;
        

        public GameHub(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        public async Task ChallengeUser(ChallengeUserModel model)
        {
        }
        public async Task ChallengeUserCallback(ChallengeUserModel model)
        {

        }
        public override async Task OnConnectedAsync()
        {
            lock(_roomManager)
            {
                _roomManager.ConnectedUsers.Add(Context.ConnectionId, Context.User.Identity.Name);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            lock(_roomManager)
            {
                _roomManager.ConnectedUsers.Remove(Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        

    }
}