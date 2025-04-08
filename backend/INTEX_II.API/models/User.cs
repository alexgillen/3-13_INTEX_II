// File: /backend/CineNiche.API/Models/User.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace CineNiche.API.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        
        [StringLength(100)]
        public string? FirstName { get; set; }
        
        [StringLength(100)]
        public string? LastName { get; set; }
        
        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
        
        // External auth provider identifier (Stytch user ID)
        [Required]
        [StringLength(100)]
        public string ExternalAuthId { get; set; }
        
        [StringLength(255)]
        public string? ProfileImageUrl { get; set; }
        
        [Range(0, 120)]
        public int? Age { get; set; }
        
        [StringLength(50)]
        public string? Gender { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "User";
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLogin { get; set; }
        
        // Navigation Property for Ratings
        public virtual ICollection<UserRating>? UserRatings { get; set; }
    }
}