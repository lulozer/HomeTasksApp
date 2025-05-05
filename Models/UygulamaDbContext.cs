using Microsoft.EntityFrameworkCore;

namespace HomeTasksApp.Models
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Gorev> Gorevler { get; set; }
        public DbSet<Household> Households { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // AssignedUser ile ilişki
            modelBuilder.Entity<Gorev>()
                .HasOne(g => g.AssignedUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(g => g.AssignedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // CompletedByUser ile ilişki
            modelBuilder.Entity<Gorev>()
                .HasOne(g => g.CompletedByUser)
                .WithMany(u => u.CompletedTasks)
                .HasForeignKey(g => g.CompletedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

