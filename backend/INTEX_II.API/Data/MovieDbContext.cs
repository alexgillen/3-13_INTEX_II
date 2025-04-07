//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Mission11.API.Data
//{
//    public class MovieDbContext : DbContext
//    {
//        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
//        {
//        }
//        public DbSet<Movie> Movies { get; set; }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mission11.API.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }

        // DbSet property mapping to the movies_titles table
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the Movie entity to the 'movies_titles' table in the database
            modelBuilder.Entity<Movie>().ToTable("movies_titles");

            base.OnModelCreating(modelBuilder);
        }
    }
}
