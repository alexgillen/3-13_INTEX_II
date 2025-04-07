// File: /backend/CineNiche.API/Models/User.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace CineNiche.API.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        // External auth provider identifier (Stytch user ID)
        [Required]
        [StringLength(100)]
        public string ExternalAuthId { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
        
        [StringLength(100)]
        public string FirstName { get; set; }
        
        [StringLength(100)]
        public string LastName { get; set; }
        
        [StringLength(2000)]
        public string ProfileImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLogin { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "User"; // Default role
    }
}