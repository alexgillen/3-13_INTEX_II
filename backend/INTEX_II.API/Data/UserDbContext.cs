// File: /backend/CineNiche.API/Data/UserDbContext.cs
using System;
using Microsoft.EntityFrameworkCore;
using CineNiche.API.Models;

namespace CineNiche.API.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Index for faster lookups
            modelBuilder.Entity<User>()
                .HasIndex(u => u.ExternalAuthId)
                .IsUnique();
                
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
                
            // Add seed data for admin user (optional)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ExternalAuthId = "admin-external-id", // Replace with actual Stytch ID when available
                    Email = "admin@cineniche.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}