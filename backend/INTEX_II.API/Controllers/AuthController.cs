// File: /backend/CineNiche.API/Controllers/AuthController.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CineNiche.API.Models;
using CineNiche.API.Services;
using CineNiche.Auth.Services;

namespace CineNiche.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IStytchService _stytchService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(
            IStytchService stytchService,
            ITokenService tokenService,
            IUserService userService)
        {
            _stytchService = stytchService;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Validate request
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            // Check if user already exists
            var existingUser = await _userService.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            // Create user in Stytch
            var stytchResult = await _stytchService.CreateUserAsync(
                request.Email, 
                request.Password,
                request.FirstName ?? "",
                request.LastName ?? ""
            );

            if (!stytchResult.Success)
            {
                return StatusCode(500, new { message = stytchResult.Error });
            }

            // Create user in our database
            var user = new User
            {
                ExternalAuthId = stytchResult.UserId,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = "User" // Default role is User
            };

            var userId = await _userService.CreateUserAsync(user);
            if (userId == Guid.Empty)
            {
                return StatusCode(500, new { message = "Failed to create user record" });
            }

            // Send email verification
            await _stytchService.SendEmailVerificationAsync(request.Email);

            return Ok(new { 
                message = "User registered successfully", 
                userId = userId,
                emailVerificationSent = true
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Authenticate with Stytch
            var authResult = await _stytchService.AuthenticateByEmailAsync(request.Email, request.Password);
            if (!authResult.Success)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Get user from our database
            var user = await _userService.GetUserByExternalIdAsync(authResult.UserId);
            if (user == null)
            {
                // If user exists in Stytch but not in our DB, create them
                user = new User
                {
                    ExternalAuthId = authResult.UserId,
                    Email = request.Email,
                    Role = "User" // Default role
                };
                
                await _userService.CreateUserAsync(user);
            }

            // Check if user is active
            if (!user.IsActive)
            {
                return Unauthorized(new { message = "This account has been deactivated" });
            }

            // Update last login time
            user.LastLogin = DateTime.UtcNow;
            await _userService.UpdateUserAsync(user);

            // Generate a token with the correct role using TokenService
            var token = _tokenService.GenerateJwtToken(authResult.UserId, user.Role);

            return Ok(new
            {
                userId = user.Id,
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                role = user.Role,
                token = token
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            if (string.IsNullOrEmpty(request.SessionId))
            {
                return BadRequest(new { message = "Session ID is required" });
            }

            var success = await _stytchService.RevokeSessionAsync(request.SessionId);
            
            return Ok(new { success = success });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] ResetPasswordRequest request)
        {
            var result = await _stytchService.SendPasswordResetEmailAsync(request.Email);
            
            return Ok(new { 
                success = result.Success, 
                message = result.Success ? "Password reset email sent" : result.Error 
            });
        }
    }

    // Request classes
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LogoutRequest
    {
        public string SessionId { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
    }
}