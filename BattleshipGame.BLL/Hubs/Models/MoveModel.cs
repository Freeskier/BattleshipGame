namespace BattleshipGame.BLL.Hubs.Models
{
    public class MoveModel
    {
        public string RoomID {get; set;}
        public int X {get; set;}
        public int Y {get; set;}
        public bool AutoPlay {get; set;}
    }
}