using Microsoft.EntityFrameworkCore;
using MysticBoostTodo.Core.Entities;

namespace MysticBoostTodo.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Todo entity
            modelBuilder.Entity<Todo>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Todo>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}