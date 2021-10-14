using System;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Player
    {
        public string ID {get; set;}

        public Player()
        {
            ID = new Guid().ToString();
        }
    }
}