using System;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Player
    {
        public string ID {get; set;}
        public Board Board {get; set;}

        public Player(string connectionID)
        {
            ID = connectionID;
        }
    }
}