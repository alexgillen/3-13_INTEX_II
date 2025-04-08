// File: /backend/CineNiche.API/Services/RatingService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CineNiche.API.Data;
using CineNiche.API.Models;

namespace CineNiche.API.Services
{
    public class RatingService : IRatingService
    {
        private readonly RatingDbContext _context;
        private readonly ILogger<RatingService> _logger;

        public RatingService(RatingDbContext context, ILogger<RatingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserRating> GetRatingByIdAsync(Guid id)
        {
            try
            {
                return await _context.UserRatings.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rating with ID {RatingId}", id);
                return null;
            }
        }

        public async Task<UserRating> GetUserRatingForShowAsync(Guid userId, string showId)
        {
            try
            {
                return await _context.UserRatings
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.ShowId == showId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rating for user {UserId} and show {ShowId}", userId, showId);
                return null;
            }
        }

        public async Task<List<UserRating>> GetAllRatingsByUserAsync(Guid userId)
        {
            try
            {
                return await _context.UserRatings
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all ratings for user {UserId}", userId);
                return new List<UserRating>();
            }
        }

        public async Task<List<UserRating>> GetAllRatingsForShowAsync(string showId)
        {
            try
            {
                return await _context.UserRatings
                    .Where(r => r.ShowId == showId)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all ratings for show {ShowId}", showId);
                return new List<UserRating>();
            }
        }

        public async Task<UserRating> AddOrUpdateRatingAsync(UserRating rating)
        {
            try
            {
                // Check if a rating already exists
                var existingRating = await _context.UserRatings
                    .FirstOrDefaultAsync(r => r.UserId == rating.UserId && r.ShowId == rating.ShowId);

                if (existingRating != null)
                {
                    // Update existing rating
                    existingRating.Rating = rating.Rating;
                    existingRating.CreatedAt = DateTime.UtcNow;
                    
                    _context.UserRatings.Update(existingRating);
                    await _context.SaveChangesAsync();
                    
                    return existingRating;
                }
                else
                {
                    // Create new rating
                    if (rating.Id == Guid.Empty)
                    {
                        rating.Id = Guid.NewGuid();
                    }
                    
                    rating.CreatedAt = DateTime.UtcNow;
                    
                    await _context.UserRatings.AddAsync(rating);
                    await _context.SaveChangesAsync();
                    
                    return rating;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding/updating rating for user {UserId} and show {ShowId}", 
                    rating.UserId, rating.ShowId);
                throw;
            }
        }

        public async Task<bool> DeleteRatingAsync(Guid ratingId)
        {
            try
            {
                var rating = await _context.UserRatings.FindAsync(ratingId);
                if (rating == null)
                {
                    return false;
                }

                _context.UserRatings.Remove(rating);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting rating {RatingId}", ratingId);
                return false;
            }
        }

        public async Task<bool> DeleteUserRatingForShowAsync(Guid userId, string showId)
        {
            try
            {
                var rating = await _context.UserRatings
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.ShowId == showId);
                    
                if (rating == null)
                {
                    return false;
                }

                _context.UserRatings.Remove(rating);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting rating for user {UserId} and show {ShowId}", userId, showId);
                return false;
            }
        }

        public async Task<double> GetAverageRatingForShowAsync(string showId)
        {
            try
            {
                var ratings = await _context.UserRatings
                    .Where(r => r.ShowId == showId)
                    .Select(r => r.Rating)
                    .ToListAsync();
                    
                if (ratings.Count == 0)
                {
                    return 0;
                }
                
                return ratings.Average();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating average rating for show {ShowId}", showId);
                return 0;
            }
        }

        public async Task<Dictionary<int, int>> GetRatingDistributionForShowAsync(string showId)
        {
            try
            {
                var ratings = await _context.UserRatings
                    .Where(r => r.ShowId == showId)
                    .GroupBy(r => r.Rating)
                    .Select(g => new { Rating = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Rating, x => x.Count);
                    
                // Ensure all ratings (1-5) are represented in the dictionary
                var distribution = new Dictionary<int, int>();
                for (int i = 1; i <= 5; i++)
                {
                    distribution[i] = ratings.ContainsKey(i) ? ratings[i] : 0;
                }
                
                return distribution;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating rating distribution for show {ShowId}", showId);
                return new Dictionary<int, int>();
            }
        }
    }
}