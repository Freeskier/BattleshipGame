using System.Collections.Generic;

namespace BattleshipGame.BLL.Hubs.Models
{
    public class MoveResponseModel
    {
        public string MyBoard {get; set;}
        public string EnemyBoard {get; set;}
        public IEnumerable<ShipStatsModel> MyShipStats {get; set;}
        public IEnumerable<ShipStatsModel> EnemyShipStats {get; set;}
        public bool OnMove {get; set;}
    }
}