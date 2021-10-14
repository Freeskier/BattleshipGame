using BattleshipGame.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.DAL.Database
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasKey(x => x.ID);
        }
    }
}