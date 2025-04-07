// File: /backend/CineNiche.API/Services/IUserService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CineNiche.API.Models;

namespace CineNiche.API.Services
{
    public interface IUserService
    {
        // Core user operations
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetUserByExternalIdAsync(string externalId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        
        // Role management
        Task<bool> AssignRoleAsync(Guid userId, string role);
        Task<List<User>> GetUsersByRoleAsync(string role);
        
        // User status management
        Task<bool> UpdateLastLoginAsync(Guid userId);
        Task<bool> ActivateUserAsync(Guid userId);
        Task<bool> DeactivateUserAsync(Guid userId);
    }
}