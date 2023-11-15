using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GameQueue.AuthTokensCache.Authorization;

internal class CacheTokenHandler : AuthorizationHandler<CacheTokenRequirement>
{
    private readonly RedisAuthTokensCache cache;

    public CacheTokenHandler(RedisAuthTokensCache cache)
        => this.cache = cache;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CacheTokenRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.Sid);
        var userNameClaim = context.User.FindFirst(ClaimTypes.Name);
        if (userIdClaim == null || userNameClaim == null)
        {
            context.Fail();
            return;
        }
        var userIdStr = userIdClaim.Value;
        var userNameStr = userNameClaim.Value;
        var savedUserName = await cache.GetKey(userIdStr);

        if (userNameStr != savedUserName)
        {
            context.Fail();
            return;
        }
        context.Succeed(requirement);
    }
}
