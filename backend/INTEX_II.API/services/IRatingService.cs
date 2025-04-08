// File: /backend/CineNiche.API/Services/IRatingService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CineNiche.API.Models; // Assuming UserRating model is here

namespace CineNiche.API.Services
{
    public interface IRatingService
    {
        Task<UserRating> GetRatingByIdAsync(Guid id);
        Task<UserRating> GetUserRatingForShowAsync(Guid userId, string showId);
        Task<List<UserRating>> GetAllRatingsByUserAsync(Guid userId);
        Task<List<UserRating>> GetAllRatingsForShowAsync(string showId);
        Task<UserRating> AddOrUpdateRatingAsync(UserRating rating);
        Task<bool> DeleteRatingAsync(Guid ratingId);
        Task<bool> DeleteUserRatingForShowAsync(Guid userId, string showId);
        Task<double> GetAverageRatingForShowAsync(string showId);
        Task<Dictionary<int, int>> GetRatingDistributionForShowAsync(string showId);
    }
}