using System;
using System.Linq;
using System.Threading.Tasks;
using BattleshipGame.BLL.Game.GameLogic.Interfaces;
using BattleshipGame.BLL.Hubs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BattleshipGame.BLL.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IChatHub>
    {
        public readonly IRoomManager _roomManager;
        
        public ChatHub(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        public async Task SendMessage(MessageModel model)
        {
            await Clients.All.ReceiveMessage(model);
        }

        public async Task LoggedUsers()
        {
            dynamic users;
            lock(_roomManager)
            {
                users = _roomManager.GetLoggedUsers();
            }
            await Clients.All.LoggedUsers(users);
        }

        public override async Task OnConnectedAsync()
        {
            await LoggedUsers();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await LoggedUsers();
            await base.OnDisconnectedAsync(exception);
        }
    }
}