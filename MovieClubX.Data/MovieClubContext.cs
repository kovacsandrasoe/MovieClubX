using Microsoft.EntityFrameworkCore;
using MovieClubX.Entities.Entity;

namespace MovieClubX.Data
{
    public class MovieClubContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public MovieClubContext(DbContextOptions<MovieClubContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

    }
}
