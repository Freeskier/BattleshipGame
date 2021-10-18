using System.Collections.Generic;

namespace BattleshipGame.BLL.Game.GameLogic.Interfaces
{
    public interface IRoomManager
    {
         Dictionary<string, string> ConnectedUsers {get; set;}
    }
}