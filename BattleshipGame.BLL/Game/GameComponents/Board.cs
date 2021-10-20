using System;
using System.Collections.Generic;
using System.Linq;
using BattleshipGame.BLL.Game.Enums;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Board
    {
        const int WIDTH = 10;
        const int HEIGHT = 10;
        Random random = new Random();
        public int[,] ShipsMap {get; set;}
        public int[,] ShotsMap {get; set;}
        private List<Ship> ships;


        public Board()
        {
            InitializeMap();
            CreateSetOfShips();
        }

        public int[,] ReturnMapForEnemy(Board enemyBoard)
        {
            int[,] mapForReturn = new int[WIDTH, HEIGHT];
            for (int i = 0; i < mapForReturn.GetLength(0); i++)
            {
                for (int j = 0; j < mapForReturn.GetLength(1); j++)
                {
                    if(enemyBoard.ShotsMap[i,j] == (int)PointType.Shot)
                    {
                        mapForReturn[i,j] = (int)PointType.Shot;
                        if(ShipsMap[i,j] == (int)PointType.Ship)
                            mapForReturn[i,j] = (int) PointType.ShipHit;
                        if(ships.Any(s => s.IsSunk && s.ShipParts.Any(p => p.X == i && p.Y == j)))
                            mapForReturn[i,j] = (int)PointType.Sunk;
                    }
                }
            }
            return mapForReturn;
        }

        public IEnumerable<ShipStatsModel> ReturnShipStats()
        {
            return ships.Select(s => new ShipStatsModel 
            {
                Size = s.ShipParts.Count,
                HitCount = s.ShipParts.Count(q => q.Hit)
            });
        }

        public int[,] ReturnMapForMe(Board enemyBoard)
        {
            int[,] mapForReturn = new int[WIDTH, HEIGHT];
            for (int i = 0; i < mapForReturn.GetLength(0); i++)
            {
                for (int j = 0; j < mapForReturn.GetLength(1); j++)
                {
                    mapForReturn[i,j] = ShipsMap[i,j];
                    if(enemyBoard.ShotsMap[i,j] == (int)PointType.Shot)
                    {
                        mapForReturn[i,j] = (int)PointType.Shot;
                        if(ShipsMap[i,j] == (int)PointType.Ship)
                            mapForReturn[i,j] = (int) PointType.ShipHit;
                        if(ships.Any(s => s.IsSunk && s.ShipParts.Any(p => p.X == i && p.Y == j)))
                            mapForReturn[i,j] = (int)PointType.Sunk;
                    }
                }
            }
            return mapForReturn;
        }

        private void ShipSunk(List<(int x, int y)> parts)
        {
            foreach(var i in parts)
            {
                ShipsMap[i.x, i.y] = (int)PointType.Sunk;
            }
        }

        private void InitializeMap()
        {
            ShipsMap = new int[WIDTH, HEIGHT];
            ShotsMap = new int[WIDTH, HEIGHT];
            ships = new List<Ship>();
        }

        public bool SetPoint(PointType point, int x, int y)
        {
            if(ShotsMap[x,y] == (int)point)
                throw new Exception($"Point x: {x} y: {y} is already set to given value.");

            ShotsMap[x,y] = (int)point;
            foreach(var ship in ships)
            {
                ship.SetPart(x, y);
            }
            if(ShipsMap[x,y] == (int)PointType.Ship || ShipsMap[x,y] == (int)PointType.Sunk)
                return true;
            else 
                return false;
        }

        public bool IsGameOver
        {
            get => ships.All(x => x.IsSunk);
        }

        public void CreateSetOfShips()
        {
            CreateShip(4);
            CreateShip(3);
            CreateShip(3);
            CreateShip(2);
            CreateShip(2);
            CreateShip(2);
            CreateShip(1);
            CreateShip(1);
            CreateShip(1);
            CreateShip(1);

        }

        private void CreateShip(int size)
        {
            bool horizontalDir = RandomBool;
            Ship ship = new Ship();
            ship.OnShipSunk += ShipSunk;

            int xPos = random.Next(0, 10);
            int yPos = random.Next(0, 10);

            int lastXPos = -1;
            int lastYPos = -1;
           
            for (int i = 0; i < size; i++)
            {
                if(CheckIfAroundEmpty(xPos, yPos, lastXPos, lastYPos))
                {
                    ship.AddPart(new PartOfShip(xPos, yPos));
                    lastXPos = xPos;
                    lastYPos = yPos;
                    if(horizontalDir) 
                        xPos++;
                    else
                        yPos++;
                }
                else
                {
                    CreateShip(size);
                    return;
                }
            }

            foreach(var p in ship.ShipParts)
            {
                ShipsMap[p.X, p.Y] = (int) PointType.Ship;
            }
            ships.Add(ship);
        }


        private bool CheckIfAroundEmpty(int x, int y, int lastX, int lastY)
        {
            if(x > WIDTH - 1 || x < 0)
                return false;
            if(y > HEIGHT - 1 || y < 0)
                return false;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if(i >= 0 && i < WIDTH  && j >= 0 && j < HEIGHT)
                    { 
                        if(i == lastX && j == lastY)
                            continue;
                        if(ShipsMap[i, j] != 0 ) 
                            return false;
                    }
                }
            }
            return true;
        }

        private bool RandomBool => random.Next(0,10) >= 5;
        private int Direction => random.Next(0, 10) >= 5? 1 : -1;

        public void Print()
        {
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    Console.Write(ShipsMap[i,j] + " ");
                }
                Console.WriteLine("\n");
            }
        }
    }

}