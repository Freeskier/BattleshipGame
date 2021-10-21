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
            ID = Guid.NewGuid().ToString();
        }

        public bool IsGameOver(out string winningUser)
        {
            if(PlayerA.Board.IsGameOver)
            {
                winningUser = PlayerB.Username;
                return true;
            }
            else if(PlayerB.Board.IsGameOver)
            {
                winningUser = PlayerA.Username;
                return true;
            }
            winningUser = "";
            return false;
        }
    }
}