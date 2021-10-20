using System;
using System.Collections.Generic;
using BattleshipGame.BLL.Game.Enums;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Player
    {
        public string ConnectionID {get; set;}
        public string Username {get; set;}
        public Board Board {get; set;}

        public Player(string connectionID, string username)
        {
            ConnectionID = connectionID;
            Username = username;
            Board = new Board();
        }

        public Player()
        {}



        public bool RecieveShoot(int x, int y)
        {
            return Board.SetPoint(PointType.Shot, x, y);
        }

        public int[,] GetBoardForMe(Board enemyBoard)
        {
            return Board.ReturnMapForMe(enemyBoard);
        }

        public int[,] GetBoardForEnemy(Board enemyBoard)
        {
            return Board.ReturnMapForEnemy(enemyBoard);
        }

        public IEnumerable<ShipStatsModel> GetShipStats()
        {
            return Board.ReturnShipStats();
        }
    }
}