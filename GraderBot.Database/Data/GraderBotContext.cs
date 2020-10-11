using Microsoft.EntityFrameworkCore;

namespace GraderBot.Database.Data
{
    using Models;

    public class GraderBotContext : DbContext
    {
        public GraderBotContext()
        { }
        public GraderBotContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Problem> Problems { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Configuration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(ent =>
            {
                ent
                    .HasIndex(e => e.Username)
                    .IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
