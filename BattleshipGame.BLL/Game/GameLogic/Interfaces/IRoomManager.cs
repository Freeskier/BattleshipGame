using System.Collections.Generic;
using System.Threading.Tasks;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Game.GameLogic.Interfaces
{
    public interface IRoomManager
    {
        Dictionary<string, string> ConnectedUsers {get; set;}
        IEnumerable<LoggedUserModel> GetLoggedUsers();
        void AddUserToConnected(string connectionID, string username);
        string GetUserConnectionID(string username);
        string CreateRoom(string userA, string userB, out string playerAconnID, out string playerBconnID);
        MoveResponseModel ProcessMove(MoveModel model, string connectionId, out string enemyConnID);
    }
}