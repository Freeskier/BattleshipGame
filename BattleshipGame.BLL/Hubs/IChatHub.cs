using System.Collections.Generic;
using System.Threading.Tasks;
using BattleshipGame.BLL.Hubs.Models;

namespace BattleshipGame.BLL.Hubs
{
    public interface IChatHub
    {
        Task SendMessage(MessageModel model);
        Task ReceiveMessage(MessageModel model);
        Task LoggedUsers(IEnumerable<LoggedUserModel> users);
    }
}