// File: /backend/CineNiche.API/Controllers/UserController.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CineNiche.API.Models;
using CineNiche.API.Services;
using CineNiche.Auth.Services;

namespace CineNiche.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStytchService _stytchService;

        public UserController(IUserService userService, IStytchService stytchService)
        {
            _userService = userService;
            _stytchService = stytchService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        [Route("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetUsersByRoleAsync("User");
            return Ok(users);
        }

        [HttpGet]
        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            // Get the user ID from claims
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "Invalid token" });
            }

            // Get user from database
            var user = await _userService.GetUserByExternalIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                profileImageUrl = user.ProfileImageUrl,
                role = user.Role,
                isActive = user.IsActive,
                createdAt = user.CreatedAt,
                lastLogin = user.LastLogin
            });
        }

        [HttpPut]
        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateProfileRequest request)
        {
            // Get the user ID from claims
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "Invalid token" });
            }

            // Get user from database
            var user = await _userService.GetUserByExternalIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Update user properties
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            if (!string.IsNullOrEmpty(request.ProfileImageUrl))
            {
                user.ProfileImageUrl = request.ProfileImageUrl;
            }

            // Save changes
            await _userService.UpdateUserAsync(user);

            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [Route("change-role")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeRoleRequest request)
        {
            // Validate role
            if (request.Role != "User" && request.Role != "Admin")
            {
                return BadRequest(new { message = "Invalid role" });
            }

            // Get user by ID
            var user = await _userService.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Update role
            var result = await _userService.AssignRoleAsync(request.UserId, request.Role);
            if (!result)
            {
                return StatusCode(500, new { message = "Failed to update user role" });
            }

            return Ok(new { message = "User role updated successfully" });
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [Route("activate")]
        public async Task<IActionResult> ActivateUser([FromBody] UserStatusRequest request)
        {
            var result = await _userService.ActivateUserAsync(request.UserId);
            if (!result)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User activated successfully" });
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [Route("deactivate")]
        public async Task<IActionResult> DeactivateUser([FromBody] UserStatusRequest request)
        {
            var result = await _userService.DeactivateUserAsync(request.UserId);
            if (!result)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User deactivated successfully" });
        }
    }

    // Request classes
    public class UpdateProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
    }

    public class ChangeRoleRequest
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }
    }

    public class UserStatusRequest
    {
        public Guid UserId { get; set; }
    }
}