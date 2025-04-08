// File: /backend/CineNiche.API/Services/UserService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CineNiche.API.Data;
using CineNiche.API.Models;

namespace CineNiche.API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateUserAsync(User user)
        {
            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return user.Id;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> GetUserByExternalIdAsync(string externalId)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.ExternalAuthId == externalId);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return false;

            try
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AssignRoleAsync(Guid userId, string role)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.Role = role;
            
            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActivateUserAsync(Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.IsActive = true;
            
            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeactivateUserAsync(Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.IsActive = false;
            
            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            return await _dbContext.Users
                .Where(u => u.Role == role)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}