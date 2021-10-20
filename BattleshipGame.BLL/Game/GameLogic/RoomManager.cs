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
        public Dictionary<string, string> ConnectedUsers {get; set;}
        public Dictionary<string, Room> Rooms {get; set;}
        private readonly IGameAI _gameAI;

        public RoomManager(IGameAI gameAI)
        {
            _gameAI = gameAI;
            ConnectedUsers = new Dictionary<string, string>();
            Rooms = new Dictionary<string, Room>();
            ConnectedUsers.Add("asdasd", "BOT");
            Rooms.Add("asdasdasd", new Room(){PlayerB = new Player{Username = "BOT", ConnectionID = "123"}});
        }

        public IEnumerable<LoggedUserModel> GetLoggedUsers()
        {
            return ConnectedUsers.Select(u => new LoggedUserModel 
            {
                Username = u.Value,
                InGame = Rooms.Any(x => x.Value.PlayerA?.Username == u.Value || x.Value.PlayerB?.Username == u.Value)
            });
        }

        public void AddUserToConnected(string connectionID, string username)
        {
            ConnectedUsers.Add(connectionID, username);
        }
        public void RemoveUserFromConnected(string connectionID)
        {
            ConnectedUsers.Remove(connectionID);
        }

        public string GetUserConnectionID(string username)
            => ConnectedUsers.FirstOrDefault(x => x.Value == username).Key;

        public string CreateRoom(string userA, string userB, out string playerAconnID, out string playerBconnID)
        {
            var room = new Room {
                PlayerA = new Player(GetUserConnectionID(userA), userA),
                PlayerB = new Player(GetUserConnectionID(userB), userB)
            };
            Rooms.Add(room.ID, room);
            playerAconnID = room.PlayerA.ConnectionID;
            playerBconnID = room.PlayerB.ConnectionID;
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