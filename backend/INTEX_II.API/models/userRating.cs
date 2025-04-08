// File: /backend/CineNiche.API/Models/UserRating.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CineNiche.API.Models;

namespace CineNiche.API.Models
{
    public class UserRating
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public string ShowId { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties for Entity Framework
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        
        [ForeignKey("ShowId")]
        public virtual Movie? Movie { get; set; }
    }
}