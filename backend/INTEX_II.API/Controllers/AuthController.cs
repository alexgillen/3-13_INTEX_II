// File: /backend/CineNiche.API/Controllers/AuthController.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CineNiche.API.Models;
using CineNiche.API.Services;
using CineNiche.Auth.Services;
using Microsoft.Extensions.Logging;

namespace CineNiche.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IStytchService _stytchService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IStytchService stytchService,
            ITokenService tokenService,
            IUserService userService,
            ILogger<AuthController> logger)
        {
            _stytchService = stytchService;
            _tokenService = tokenService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Validate request
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "Email and password are required" });
                }

                _logger.LogInformation($"Registration attempt for email: {request.Email}");

                // Check if user already exists
                var existingUser = await _userService.GetUserByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning($"Registration failed: Email {request.Email} already exists");
                    return BadRequest(new { message = "User with this email already exists" });
                }

                _logger.LogInformation($"Creating user in Stytch for email: {request.Email}");
                
                // Create user in Stytch
                var stytchResult = await _stytchService.CreateUserAsync(
                    request.Email, 
                    request.Password,
                    request.FirstName ?? "",
                    request.LastName ?? ""
                );

                if (!stytchResult.Success)
                {
                    _logger.LogError($"Stytch user creation failed: {stytchResult.Error}");
                    return StatusCode(500, new { message = $"Stytch API error: {stytchResult.Error}" });
                }

                _logger.LogInformation($"User created in Stytch with ID: {stytchResult.UserId}");

                // Create user in our database
                var userToCreate = new User
                {
                    ExternalAuthId = stytchResult.UserId,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Role = "User" // Default role is User
                };

                var createdUser = await _userService.CreateUserAsync(userToCreate);
                if (createdUser == null)
                {
                    _logger.LogError($"Failed to create user in local database after Stytch registration");
                    return StatusCode(500, new { message = "Failed to create user record after Stytch registration" });
                }

                _logger.LogInformation($"User created in local database with ID: {createdUser}");

                // Send email verification
                var verificationResult = await _stytchService.SendEmailVerificationAsync(request.Email);
                if (!verificationResult.Success)
                {
                    _logger.LogWarning($"Failed to send verification email: {verificationResult.Error}");
                }

                return Ok(new { 
                    message = "User registered successfully", 
                    userId = createdUser,
                    emailVerificationSent = verificationResult.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration");
                return StatusCode(500, new { message = "An unexpected error occurred during registration" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // DEV MODE ONLY: For development purposes, bypass Stytch authentication
            // and allow login with just an email lookup in our local database
            _logger.LogWarning("⚠️ USING DEVELOPMENT MODE LOGIN - NOT SECURE FOR PRODUCTION ⚠️");
            
            try
            {
                // Search for user by email in our database
                var user = await _userService.GetUserByEmailAsync(request.Email);
                
                if (user == null)
                {
                    _logger.LogInformation($"Login failed: User with email {request.Email} not found");
                    return Unauthorized(new { message = "Invalid credentials" });
                }
                
                _logger.LogInformation($"Login successful for user: {user.Email} (ID: {user.Id})");
                
                // Check if user is active
                if (!user.IsActive)
                {
                    return Unauthorized(new { message = "This account has been deactivated" });
                }
                
                // Update last login time
                user.LastLogin = DateTime.UtcNow;
                await _userService.UpdateUserAsync(user);
                
                // Generate a token with the correct role using TokenService
                // Ensure that ExternalAuthId is not empty for token generation
                if (string.IsNullOrEmpty(user.ExternalAuthId))
                {
                    _logger.LogWarning($"User {user.Id} has no ExternalAuthId. Setting a temporary one for development.");
                    user.ExternalAuthId = user.Id.ToString();
                    await _userService.UpdateUserAsync(user);
                }
                
                // Debug the values being used for token generation
                _logger.LogInformation($"Generating token with: UserId={user.ExternalAuthId}, Role={user.Role}");
                var token = _tokenService.GenerateJwtToken(user.ExternalAuthId, user.Role);
                
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new { message = "An unexpected error occurred during login" });
            }
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

        [HttpGet("debug-token")]
        public IActionResult DebugToken()
        {
            try
            {
                // Get the Authorization header
                if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    return BadRequest(new { message = "No Authorization header found" });
                }
                
                // Extract the token
                var token = authHeader.ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "No token found in Authorization header" });
                }
                
                // Decode the token
                string userId, role;
                bool isValid = _tokenService.ReadTokenInfo(token, out userId, out role);
                
                return Ok(new
                {
                    isValid,
                    userId,
                    role,
                    message = isValid ? "Token is valid" : "Token is invalid"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error parsing token: {ex.Message}" });
            }
        }
    }

    // Request classes
    public class RegisterRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LogoutRequest
    {
        public string? SessionId { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
    }
}