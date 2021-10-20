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
        void RemoveUserFromConnected(string connectionID);
        string GetUserConnectionID(string username);
        string CreateRoom(string userA, string userB, out string playerAconnID, out string playerBconnID);
        bool ProcessMove(MoveModel model, string connectionId, out string enemyConnID);
        void GetMoveResponseData(string roomId, string connID, bool extraMove, out MoveResponseModel responseA, out MoveResponseModel responseB);
    }
}