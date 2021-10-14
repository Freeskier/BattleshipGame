using System;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Lobby
    {
        public string ID {get; set;}
        public DateTime CreationDate {get; set;}

        public Lobby()
        {
            CreationDate = DateTime.Now;
            ID = new Guid().ToString();
        }
    }
}