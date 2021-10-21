using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipGame.BLL.Game.GameLogic.Interfaces;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Game.GameLogic
{
    public class LobbyManager : ILobbyManager
    {
        public Dictionary<string, (string chatID, string gameID)> ConnectedUsers {get; set;}
        private readonly IRoomManager _roomManager;

        public LobbyManager(IRoomManager roomManager)
        {
            ConnectedUsers = new Dictionary<string, (string chatID, string gameID)>();
            _roomManager = roomManager;
        }
        public void JoinLobby(string username, string chatConnID = "", string gameConnID = "")
        {
            if(!ConnectedUsers.ContainsKey(username))
                ConnectedUsers.Add(username, (chatConnID, gameConnID));
            else
            {
                var usr = ConnectedUsers[username];
                if(gameConnID != "")
                    usr.gameID = gameConnID;
                if(chatConnID != "")
                    usr.chatID = chatConnID;
                ConnectedUsers[username] = usr;
            }
        }

        public void LeaveLobby(string connID)
        {
            var usr = ConnectedUsers.FirstOrDefault(x => x.Value.chatID == connID || x.Value.gameID == connID).Key;
            if(usr != null)
                ConnectedUsers.Remove(usr);
        }

        public IEnumerable<LoggedUserModel> GetLoggedUsers()
        {
            return ConnectedUsers.Select(u => new LoggedUserModel 
            {
                Username = u.Key,
                InGame = _roomManager.Rooms.Any(x => x.Value.PlayerA?.Username == u.Key || x.Value.PlayerB?.Username == u.Key)
            });
        }

        public string GetUsername(string connID)
            => ConnectedUsers.FirstOrDefault(u => u.Value.gameID == connID).Key;
        

        public string GetChatConnID(string connID)
            => ConnectedUsers.FirstOrDefault(u => u.Value.gameID == connID).Value.chatID;
        
    }
}