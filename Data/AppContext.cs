using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Data.Models;
namespace Data
{
    public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasMany(c => c.Followers).WithMany(x => x.Subscriptions);
            modelBuilder.Entity<Group>().HasMany(c => c.Authors).WithMany(x => x.GroupsToPost);

            base.OnModelCreating(modelBuilder);
        }
    }
}
