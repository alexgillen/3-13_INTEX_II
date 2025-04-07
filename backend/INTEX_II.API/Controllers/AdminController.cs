// File: /backend/CineNiche.API/Controllers/AdminController.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CineNiche.API.Services;

namespace CineNiche.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdminUser([FromBody] CreateAdminRequest request)
        {
            // Check if user exists
            var user = await _userService.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Make user an admin
            var result = await _userService.AssignRoleAsync(request.UserId, "Admin");
            if (!result)
            {
                return StatusCode(500, new { message = "Failed to assign admin role" });
            }

            return Ok(new { message = "Admin user created successfully" });
        }

        [HttpPost("revoke-admin")]
        public async Task<IActionResult> RevokeAdminRole([FromBody] CreateAdminRequest request)
        {
            // Check if user exists
            var user = await _userService.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Make user a regular user
            var result = await _userService.AssignRoleAsync(request.UserId, "User");
            if (!result)
            {
                return StatusCode(500, new { message = "Failed to revoke admin role" });
            }

            return Ok(new { message = "Admin role revoked successfully" });
        }
    }

    public class CreateAdminRequest
    {
        public Guid UserId { get; set; }
    }
}