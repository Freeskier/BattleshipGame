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
        public readonly ILobbyManager _lobbyManager;

        public ChatHub(IRoomManager roomManager, ILobbyManager lobbyManager)
        {
            _roomManager = roomManager;
            _lobbyManager = lobbyManager;
        }

        public async Task SendMessage(MessageModel model)
        {
            await Clients.All.ReceiveMessage(model);
        }

        public async Task LoggedUsers()
        {   
            await Clients.All.LoggedUsers(_lobbyManager.GetLoggedUsers());
        }

        public async Task Disconnect()
        {
            Context.Abort();
            await LoggedUsers();
        }

        public override async Task OnConnectedAsync()
        {
            _lobbyManager.JoinLobby(Context.User.Identity.Name, Context.ConnectionId);
            await LoggedUsers();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _lobbyManager.LeaveLobby(Context.ConnectionId);
            await LoggedUsers();
            await base.OnDisconnectedAsync(exception);
        }
    }
}