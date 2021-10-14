using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Ship
    {
        public List<PartOfShip> ShipParts {get; set;}

        public Ship()
        {
            ShipParts = new List<PartOfShip>();
        }   

        public void AddPart(PartOfShip part) => ShipParts.Add(part);
        public void SetPart(int x, int y)
        {
            foreach(var part in ShipParts)
            {
                if(part.X == x && part.Y == y)
                    part.Hit = true;
            }
        }

        public bool IsSunk
        {
            get => ShipParts.All(x => x.Hit);
        }

    }

    

    public class PartOfShip
    {
        public int X {get; set;}
        public int Y {get; set;}
        public bool Hit {get; set;}
        public PartOfShip(int x, int y)
        {
            this.X = x;
            this.Y = y;
            Hit = false;
        }
    }
}