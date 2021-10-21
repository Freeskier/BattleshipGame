using System.Collections.Generic;
using System.Threading.Tasks;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Game.GameLogic.Interfaces
{
    public interface ILobbyManager
    {
        Dictionary<string, (string chatID, string gameID)> ConnectedUsers {get; set;}
        void JoinLobby(string username, string chatConnID = "", string gameConnID = "");
        void LeaveLobby(string connID);
        string GetUsername(string connID);
        string GetChatConnID(string connID);
        IEnumerable<LoggedUserModel> GetLoggedUsers();
    }
}