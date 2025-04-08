// File: /backend/CineNiche.API/Data/RatingDbContext.cs
using Microsoft.EntityFrameworkCore;
using CineNiche.API.Models;

namespace CineNiche.API.Data
{
    public class RatingDbContext : DbContext
    {
        public RatingDbContext(DbContextOptions<RatingDbContext> options) : base(options)
        {
        }
        
        public DbSet<UserRating> UserRatings { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Create a composite index to ensure a user can only rate a show once
            modelBuilder.Entity<UserRating>()
                .HasIndex(r => new { r.UserId, r.ShowId })
                .IsUnique();
        }
    }
}