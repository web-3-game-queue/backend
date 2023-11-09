using System.Text;
using GameQueue.Host.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GameQueue.Host.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetRequiredSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtConfig["Key"] ?? throw new NullReferenceException("jwtConfig[Key]"));

        var tokenValidationParameters = new TokenValidationParameters {
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };

        var authScheme = JwtBearerDefaults.AuthenticationScheme;
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = authScheme;
                options.DefaultChallengeScheme = authScheme;
                options.DefaultScheme = authScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = tokenValidationParameters;
            });

        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
