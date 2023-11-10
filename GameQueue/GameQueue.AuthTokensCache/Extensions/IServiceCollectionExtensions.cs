using GameQueue.AuthTokensCache.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameQueue.AuthTokensCache.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthWithTokenCache(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration["RedisCache:Configuration"];
                    options.InstanceName = configuration["RedisCache:Configuration"];
                })
            .AddScoped<RedisAuthTokensCache>()
            .AddScoped<IAuthorizationHandler, CacheTokenHandler>()
            .AddAuthorizationCore(options =>
                {
                    options.AddPolicy(
                        CacheTokenRequirement.Name,
                        policy => policy.Requirements.Add(new CacheTokenRequirement()));
                });
        return services;
    }
}
