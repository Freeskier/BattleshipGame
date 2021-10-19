using System.Threading.Tasks;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Hubs
{
    public interface IChatHub
    {
        Task SendMessage(MessageModel model);
        Task RecieveMessage(MessageModel model);
    }
}