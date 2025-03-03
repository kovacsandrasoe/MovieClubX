using Microsoft.EntityFrameworkCore;
using MovieClubX.Entities.Entity;

namespace MovieClubX.Data
{
    public class MovieClubContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rate> Rates { get; set; }

        public MovieClubContext(DbContextOptions<MovieClubContext> opt) : base(opt)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Rates)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

    }
}
