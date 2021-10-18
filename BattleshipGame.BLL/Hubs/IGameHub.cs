using System.Threading.Tasks;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Hubs
{
    public interface IGameHub
    {
        Task ChallengeUser(ChallengeUserModel model);
        Task ChallengeUserCallback(ChallengeUserModel model);
    }
}