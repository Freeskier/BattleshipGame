using System;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Player
    {
        public string ConnectionID {get; set;}
        public string Username {get; set;}
        public Board Board {get; set;}

        public Player(string connectionID)
        {
            ConnectionID = connectionID;
        }

        public Player()
        {
            
        }
    }
}