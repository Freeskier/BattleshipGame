using System.Threading.Tasks;
using BattleshipGame.DAL.Entities;

namespace BattleshipGame.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByLogin(string login);
    }
}