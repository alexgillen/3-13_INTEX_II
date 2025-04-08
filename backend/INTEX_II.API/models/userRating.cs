// File: /backend/CineNiche.API/Models/UserRating.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace CineNiche.API.Models
{
    public class UserRating
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public int ShowId { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}