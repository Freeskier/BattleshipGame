using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BattleshipGame.DAL.Database;
using BattleshipGame.DAL.Entities;
using BattleshipGame.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public async Task<User> GetByLogin(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
        }
    }
}