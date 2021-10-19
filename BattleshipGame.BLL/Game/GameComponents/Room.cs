using System;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Room
    {
        public string ID {get; set;}
        public DateTime CreationDate {get; set;}
        public Player PlayerA {get; set;}
        public Player PlayerB {get; set;}

        public Room()
        {
            CreationDate = DateTime.Now;
            ID = new Guid().ToString();
        }
    }
}