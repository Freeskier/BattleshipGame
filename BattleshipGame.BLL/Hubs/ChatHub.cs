using System;
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
        public static IRoomManager _roomManager;
        

        public ChatHub(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        public async Task SendMessage(MessageModel model)
        {
            await Clients.All.RecieveMessage(model);
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