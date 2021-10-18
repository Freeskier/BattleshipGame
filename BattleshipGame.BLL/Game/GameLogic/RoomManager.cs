using System.Collections.Generic;
using BattleshipGame.BLL.Game.GameLogic.Interfaces;

namespace BattleshipGame.BLL.Game.GameLogic
{
    public class RoomManager : IRoomManager
    {
        public Dictionary<string, string> ConnectedUsers {get; set;}

        public RoomManager()
        {
            ConnectedUsers = new Dictionary<string, string>();
        }

    }
}