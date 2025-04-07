// File: /backend/CineNiche.API/Services/IUserService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CineNiche.API.Models;

namespace CineNiche.API.Services
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByExternalIdAsync(string externalId);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> AssignRoleAsync(Guid userId, string role);
        Task<bool> ActivateUserAsync(Guid userId);
        Task<bool> DeactivateUserAsync(Guid userId);
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<List<User>> GetAllUsersAsync();
    }
}