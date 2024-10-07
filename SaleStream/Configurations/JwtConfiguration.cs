using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SaleStream.Configurations
{
    public static class JwtConfiguration
    {
        
        /// Configures JWT Authentication and Role-based Authorization Policies
        public static void ConfigureJwtAndRoles(this IServiceCollection services, IConfiguration configuration)
        {
            // Load Jwt settings from configuration
            var jwtSettings = new JwtSettings();
            configuration.Bind("Jwt", jwtSettings);
            services.AddSingleton(jwtSettings);

            // JWT Bearer authentication configuration
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

            // Configure role-based authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("VendorPolicy", policy => policy.RequireRole("Vendor"));
                options.AddPolicy("CSRPolicy", policy => policy.RequireRole("CSR"));
                
            });
        }
    }

[Authorize(Policy = "VendorPolicy, AdminPolicy")]
    /// Holds Jwt Settings for authentication
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
