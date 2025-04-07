// File: /backend/CineNiche.Auth/Services/ITokenService.cs
using System;

namespace CineNiche.Auth.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for the authenticated user
        /// </summary>
        /// <param name="userId">The unique identifier for the user (Stytch user ID)</param>
        /// <param name="role">The user's role (e.g., "User" or "Admin")</param>
        /// <returns>A JWT token string</returns>
        string GenerateJwtToken(string userId, string role);
        
        /// <summary>
        /// Validates a JWT token and extracts user information
        /// </summary>
        /// <param name="token">The JWT token to validate</param>
        /// <param name="userId">Output parameter that will contain the user ID if validation succeeds</param>
        /// <param name="role">Output parameter that will contain the user role if validation succeeds</param>
        /// <returns>True if the token is valid, otherwise false</returns>
        bool ValidateJwtToken(string token, out string userId, out string role);
        
        /// <summary>
        /// Extracts information from a token without fully validating it
        /// </summary>
        /// <param name="token">The JWT token to read</param>
        /// <param name="userId">Output parameter that will contain the user ID</param>
        /// <param name="role">Output parameter that will contain the user role</param>
        /// <returns>True if information was successfully extracted, otherwise false</returns>
        bool ReadTokenInfo(string token, out string userId, out string role);
    }
}