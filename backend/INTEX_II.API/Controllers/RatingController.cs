// File: /backend/CineNiche.API/Controllers/RatingController.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CineNiche.API.Models;
using CineNiche.API.Services;

namespace CineNiche.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IUserService _userService;

        public RatingController(IRatingService ratingService, IUserService userService)
        {
            _ratingService = ratingService;
            _userService = userService;
        }

        [HttpGet]
        [Route("show/{showId}")]
        public async Task<IActionResult> GetShowRatings(int showId)
        {
            var ratings = await _ratingService.GetAllRatingsForShowAsync(showId);
            var average = await _ratingService.GetAverageRatingForShowAsync(showId);
            var distribution = await _ratingService.GetRatingDistributionForShowAsync(showId);

            return Ok(new
            {
                average,
                totalRatings = ratings.Count,
                distribution,
                ratings
            });
        }

        [HttpGet]
        [Authorize]
        [Route("user/{showId}")]
        public async Task<IActionResult> GetUserRatingForShow(int showId)
        {
            // Get the user ID from claims
            var externalUserId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(externalUserId))
            {
                return BadRequest(new { message = "Invalid token" });
            }

            // Get user from database to get internal ID
            var user = await _userService.GetUserByExternalIdAsync(externalUserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var rating = await _ratingService.GetUserRatingForShowAsync(user.Id, showId);
            if (rating == null)
            {
                return NotFound(new { message = "Rating not found" });
            }

            return Ok(rating);
        }

        [HttpPost]
        [Authorize]
        [Route("rate")]
        public async Task<IActionResult> RateShow([FromBody] RateShowRequest request)
        {
            if (request.Rating < 1 || request.Rating > 5)
            {
                return BadRequest(new { message = "Rating must be between 1 and 5" });
            }

            // Get the user ID from claims
            var externalUserId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(externalUserId))
            {
                return BadRequest(new { message = "Invalid token" });
            }

            // Get user from database to get internal ID
            var user = await _userService.GetUserByExternalIdAsync(externalUserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var rating = new UserRating
            {
                UserId = user.Id,
                ShowId = request.ShowId,
                Rating = request.Rating
            };

            var result = await _ratingService.AddOrUpdateRatingAsync(rating);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize]
        [Route("{showId}")]
        public async Task<IActionResult> DeleteRating(int showId)
        {
            // Get the user ID from claims
            var externalUserId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(externalUserId))
            {
                return BadRequest(new { message = "Invalid token" });
            }

            // Get user from database to get internal ID
            var user = await _userService.GetUserByExternalIdAsync(externalUserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var result = await _ratingService.DeleteUserRatingForShowAsync(user.Id, showId);
            if (!result)
            {
                return NotFound(new { message = "Rating not found" });
            }

            return Ok(new { message = "Rating deleted successfully" });
        }

        [HttpGet]
        [Authorize]
        [Route("my-ratings")]
        public async Task<IActionResult> GetUserRatings()
        {
            // Get the user ID from claims
            var externalUserId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(externalUserId))
            {
                return BadRequest(new { message = "Invalid token" });
            }

            // Get user from database to get internal ID
            var user = await _userService.GetUserByExternalIdAsync(externalUserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var ratings = await _ratingService.GetAllRatingsByUserAsync(user.Id);
            return Ok(ratings);
        }
    }

    // Request classes
    public class RateShowRequest
    {
        public int ShowId { get; set; }
        public int Rating { get; set; }
    }
}