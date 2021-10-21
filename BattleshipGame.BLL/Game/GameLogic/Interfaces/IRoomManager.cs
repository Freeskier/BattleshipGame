using System.Collections.Generic;
using System.Threading.Tasks;
using BattleshipGame.BLL.Game.GameModels;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Game.GameLogic.Interfaces
{
    public interface IRoomManager
    {
        Dictionary<string, Room> Rooms {get; set;}
        string CreateRoom(string playerAconnID, string playerBconnID, string userA, string userB);
        bool ProcessMove(MoveModel model, string connectionId, out string enemyConnID);
        void GetMoveResponseData(string roomId, string connID, bool extraMove, out MoveResponseModel responseA, out MoveResponseModel responseB);
        bool IsGameOver(string roomID, out string winningUser);
        void DeleteRoomWithUser(string connectionId);
        string GetUserEnemyConnID(string connectionId);
    }
}