using System;
using System.Collections.Generic;
using System.Linq;
using BattleshipGame.BLL.Game.Enums;

namespace BattleshipGame.BLL.Game.GameModels
{
    public class Board
    {
        const int WIDTH = 10;
        const int HEIGHT = 10;
        Random random = new Random();
        int[,] map;
        List<Ship> ships;

        public Board()
        {
            InitializeMap();
            CreateSetOfShips();
        }

        

        private void InitializeMap()
        {
            map = new int[WIDTH, HEIGHT];
            ships = new List<Ship>();
        }

        public void SetPoint(PointType point, int x, int y)
        {
            map[x,y] = (int)point;
            foreach(var ship in ships)
            {
                ship.SetPart(x, y);
            }
        }

        public bool IsGameOver
        {
            get => ships.All(x => x.IsSunk);
        }

        private void CreateSetOfShips()
        {
            for (int i = 0; i < 5; i++)
            {
                CreateShip(random.Next(2,5));
            }
        }

        private void CreateShip(int size)
        {
            bool horizontalDir = RandomBool;
            Ship ship = new Ship();

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
                map[p.X, p.Y] = (int) PointType.Ship;
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
                        if(map[i, j] != 0 ) 
                            return false;
                    }
                }
            }
            return true;
        }

        private bool RandomBool => random.Next(0,10) > 5;
        private int Direction => random.Next(0, 10) > 5? 1 : -1;

        public void Print()
        {
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    Console.Write(map[i,j] + " ");
                }
                Console.WriteLine("\n");
            }
        }
    }

}