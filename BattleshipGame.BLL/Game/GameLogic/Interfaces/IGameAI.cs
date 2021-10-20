using BattleshipGame.BLL.Game.GameModels;

namespace BattleshipGame.BLL.Game.GameLogic.Interfaces
{
    public interface IGameAI
    {
         (int x, int y) ComputeMove(int[,] enemyMap);
    }
}