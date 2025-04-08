// File: /backend/CineNiche.Auth/Configuration/StytchConfig.cs
using System;

namespace CineNiche.Auth.Configuration
{
    public class StytchConfig
    {
        public const string SectionName = "Stytch";
        
        public string ProjectId { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string PublicToken { get; set; } = string.Empty;
        
        // Base URL for API calls - 
        public string BaseUrl { get; set; } = "https://test.stytch.com/v1/";
        
        // JWT Settings
        public string JwtSigningKey { get; set; } = string.Empty;
        public int JwtExpiryMinutes { get; set; } = 60;
        
        // Optional: Redirect URLs for authentication flows
        public string LoginRedirectUrl { get; set; } = string.Empty;
        public string SignupRedirectUrl { get; set; } = string.Empty;
    }
}