using System;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Room
    {
        public string ID {get; set;}
        public DateTime CreationDate {get; set;}

        public Room()
        {
            CreationDate = DateTime.Now;
            ID = new Guid().ToString();
        }
    }
}