using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Ship
    {
        public List<PartOfShip> ShipParts {get; set;}
        public event Action<List<(int x, int y)>> OnShipSunk;

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

            if(IsSunk)
                OnShipSunk?.Invoke(ShipParts.Select(p => (x = p.X, y = p.Y)).ToList());

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