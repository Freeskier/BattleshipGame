using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipGame.BLL.Game.GameLogic.Interfaces;
using BattleshipGame.BLL.Game.GameModels;
using BattleshipGame.BLL.Hubs.Models;
using Newtonsoft.Json;

namespace BattleshipGame.BLL.Game.GameLogic
{
    public class RoomManager : IRoomManager
    {
        public Dictionary<string, Room> Rooms {get; set;}
        private readonly IGameAI _gameAI;

        public RoomManager(IGameAI gameAI)
        {
            _gameAI = gameAI;
            Rooms = new Dictionary<string, Room>();
        }



        public string CreateRoom(string userAConnID, string userBConnID, string userA, string userB)
        {
            var room = new Room {
                PlayerA = new Player(userAConnID, userA),
                PlayerB = new Player(userBConnID, userB)
            };
            Rooms.Add(room.ID, room);
            return room.ID;
        }

        public bool ProcessMove(MoveModel model, string connectionId, out string enemyConnID)
        {
            var room = Rooms[model.RoomID];
            var enemy = GetEnemy(room, connectionId);
            var player = GetPlayer(room, connectionId);
            enemyConnID = enemy.ConnectionID;
            if(model.AutoPlay)
            {
                var shoot = _gameAI.ComputeMove(enemy.GetBoardForEnemy(enemy.Board));
                return enemy.RecieveShoot(shoot.x, shoot.y);
            }
            return enemy.RecieveShoot(model.X, model.Y);
        }

        public bool IsGameOver(string roomID, out string winningUser)
            => Rooms[roomID].IsGameOver(out winningUser);
        

        public void GetMoveResponseData(string roomID, string connID, bool extraMove, out MoveResponseModel responseA, out MoveResponseModel responseB)
        {
            var room = Rooms[roomID];
            var playerA = GetPlayer(room, connID);
            var playerB = GetEnemy(room, connID);

            responseA = new MoveResponseModel 
            {
                MyBoard = JsonConvert.SerializeObject(playerA.GetBoardForMe(playerA.Board)),
                EnemyBoard = JsonConvert.SerializeObject(playerB.GetBoardForEnemy(playerB.Board)),
                MyShipStats = playerA.GetShipStats(),
                EnemyShipStats = playerB.GetShipStats(),
                OnMove = extraMove
            };

            responseB = new MoveResponseModel 
            {
                MyBoard = JsonConvert.SerializeObject(playerB.GetBoardForMe(playerB.Board)),
                EnemyBoard = JsonConvert.SerializeObject(playerA.GetBoardForEnemy(playerA.Board)),
                MyShipStats = playerB.GetShipStats(),
                EnemyShipStats = playerA.GetShipStats(),
                OnMove = !extraMove
            };
            
        }


        public void DeleteRoomWithUser(string connectionId)
        {
            var room = Rooms.FirstOrDefault(u => u.Value.PlayerA?.ConnectionID == connectionId 
                || u.Value.PlayerB?.ConnectionID == connectionId).Key;
            Rooms.Remove(room);
        }

        public string GetUserEnemyConnID(string connectionId)
        {
            var room = Rooms.FirstOrDefault(u => u.Value.PlayerA?.ConnectionID == connectionId).Value;
            if(room != null)
                return room.PlayerB.ConnectionID;
            room = Rooms.FirstOrDefault(u => u.Value.PlayerB?.ConnectionID == connectionId).Value;
            if(room != null)
                return room.PlayerA.ConnectionID;
            return "";
        }
       

        private Player GetEnemy(Room room, string connID)
        {
            if(room.PlayerA.ConnectionID.Equals(connID))
                return room.PlayerB;
            return room.PlayerA;
        }
        private Player GetPlayer(Room room, string connID)
        {
            if(room.PlayerA.ConnectionID.Equals(connID))
                return room.PlayerA;
            return room.PlayerB;
        }
    }
}