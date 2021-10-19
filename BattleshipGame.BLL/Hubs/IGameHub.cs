using System.Threading.Tasks;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Hubs
{
    public interface IGameHub
    {
        Task SendChallenge(ChallengeUserModel model);
        Task ChallengeUserCallback(ChallengeUserModel model);
        Task AcceptChallenge(ChallengeUserModel model);
        Task StartGame(StartGameModel model);
        Task MoveResponse(MoveResponseModel model);
    }
}