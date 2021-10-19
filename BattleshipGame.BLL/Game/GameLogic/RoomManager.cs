using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipGame.BLL.Game.GameLogic.Interfaces;
using BattleshipGame.BLL.Game.GameModels;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Game.GameLogic
{
    public class RoomManager : IRoomManager
    {
        public Dictionary<string, string> ConnectedUsers {get; set;}
        public Dictionary<string, Room> Rooms {get; set;}

        public RoomManager()
        {
            ConnectedUsers = new Dictionary<string, string>();
            Rooms = new Dictionary<string, Room>();
            ConnectedUsers.Add("asdasd", "BOT");
            Rooms.Add("asdasdasd", new Room(){PlayerB = new Player{Username = "BOT", ConnectionID = "123"}});
        }

        public IEnumerable<LoggedUserModel> GetLoggedUsers()
        {
            return ConnectedUsers.Select(u => new LoggedUserModel 
            {
                Username = u.Value?? "",
                InGame = Rooms.Any(x => x.Value.PlayerA?.Username == u.Value || x.Value.PlayerB?.Username == u.Value)
            });
        }

        public void AddUserToConnected(string connectionID, string username)
        {
            if(!ConnectedUsers.Values.Contains(username))
                ConnectedUsers.Add(connectionID, username);
        }

        public string GetUserConnectionID(string username)
            => ConnectedUsers.FirstOrDefault(x => x.Value == username).Key;
        
    }
}