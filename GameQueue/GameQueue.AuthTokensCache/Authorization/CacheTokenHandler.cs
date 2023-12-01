using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace GameQueue.AuthTokensCache.Authorization;

internal class CacheTokenHandler : AuthorizationHandler<CacheTokenRequirement>
{
    private readonly RedisAuthTokensCache cache;

    public CacheTokenHandler(RedisAuthTokensCache cache)
        => this.cache = cache;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CacheTokenRequirement requirement)
    {
        var ctx = (DefaultHttpContext)context.Resource!;
        var jwt = await ctx.GetTokenAsync("access_token");

        var blackListedJwt = await cache.GetKey(jwt);
        if (blackListedJwt != null)
        {
            context.Fail();
            return;
        }
        context.Succeed(requirement);
    }
}
