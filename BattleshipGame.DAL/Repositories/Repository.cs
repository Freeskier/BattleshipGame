using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipGame.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entityReturned = await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entityReturned.Entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetAsync(int entityID)
        {
            return await _context.Set<TEntity>().FindAsync(entityID);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}