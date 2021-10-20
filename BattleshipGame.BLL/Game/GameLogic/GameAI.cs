using System;
using System.Collections.Generic;
using System.Linq;
using BattleshipGame.BLL.Game.Enums;
using BattleshipGame.BLL.Game.GameLogic.Interfaces;
using BattleshipGame.BLL.Game.GameModels;

namespace BattleshipGame.BLL.Game.GameLogic
{
    public class GameAI : IGameAI
    {
        Random random = new Random();
        private int randomDirection => random.Next(0, 10) >= 5? 1 : -1;
        private bool randomBool => random.Next(0, 10) >= 5;

        public (int x, int y) ComputeMove(int[,] enemyMap)
        {
            var target = FindHit(enemyMap);
            
            if(target != (-1, -1))
            {
                var dir = FindOutDirection(enemyMap, target.x, target.y);
                dynamic coords;
                if(dir.vertical || dir.horizontal)
                    coords = FindEmpty(enemyMap, target, dir);
                else 
                    coords = RandomDirectedCoord(enemyMap, target);
                return coords;
            }
            return RandomCoord(enemyMap);
        }

        private (int x, int y) RandomCoord(int [,] map)
        {
            (int x, int y) coord = Constraint(map, (random.Next(0, 10), random.Next(0, 10)));
            if(map[coord.x, coord.y] != (int)PointType.Empty) 
                return RandomCoord(map);
            return coord;
        }

        private (int x, int y) RandomDirectedCoord(int [,] map, (int x, int y) coord) 
        { 
            (int x, int y) newCoord = randomBool? (coord.x + randomDirection, coord.y) : (coord.x, coord.y + randomDirection);
            newCoord = Constraint(map, newCoord);
            if(map[newCoord.x, newCoord.y] != (int)PointType.Empty || newCoord == coord) 
                return RandomDirectedCoord(map, coord);
            return newCoord;
        }

        private (int x, int y) FindEmpty(int[,] map, (int x, int y) coord, (bool ver, bool hor) direction)
        {
            for(int i = 1; i < 5; i++)
            {
                if(direction.hor)
                {   var c1 = Constraint(map, (coord.x + i, coord.y));
                    var c2 = Constraint(map, (coord.x - i, coord.y));
                    if(map[c1.x, c1.y] == (int)PointType.Empty)
                        return(c1.x, c1.y);
                    if(map[c2.x , c2.y] == (int)PointType.Empty)
                        return(c2.x, c2.y);
                }
                if(direction.ver)
                {
                    var c1 = Constraint(map, (coord.x, coord.y + i));
                    var c2 = Constraint(map, (coord.x, coord.y - i));
                    if(map[c1.x, c1.y] == (int)PointType.Empty)
                        return(c1.x, c1.y);
                    if(map[c2.x , c2.y] == (int)PointType.Empty)
                        return(c2.x, c2.y);
                }
            }
            return(-1, -1);
        }

        private (int x, int y) Constraint(int[,] map, (int x, int y) coord)
        {
            int retX = coord.x, retY = coord.y;
            if(coord.x < 0) retX = 0;
            if(coord.x >= map.GetLength(0)) retX = map.GetLength(0) - 1;
            if(coord.y < 0) retY = 0;
            if(coord.y >= map.GetLength(1)) retY = map.GetLength(1) - 1;
            return(retX, retY);
        }


        private (int x, int y) FindHit(int[,] enemyMap)
        {
            for (int i = 0; i < enemyMap.GetLength(0); i++)
            {
                for (int j = 0; j < enemyMap.GetLength(1); j++)
                {
                    if(enemyMap[i,j] == (int)PointType.ShipHit)
                        return (i, j);
                }
            }
            return (-1, -1);
        }

        private(bool vertical, bool horizontal) FindOutDirection(int[,] enemyMap, int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                if(i >= 0 && i < enemyMap.GetLength(0) && i != x)
                {
                    if(enemyMap[i, y] == (int)PointType.ShipHit)
                        return (false, true);
                }
            }
            for (int i = y - 1; i <= y + 1; i++)
            {
                if(i >= 0 && i < enemyMap.GetLength(1) && i != y)
                {
                    if(enemyMap[x, i] == (int)PointType.ShipHit)
                        return (true, false);
                }
            }
            return (false, false);
        }
    }
}