using System.Security.Claims;
using GameQueue.Api.Contracts.Controllers;
using GameQueue.Api.Contracts.Exceptions;
using GameQueue.AuthTokensCache;
using GameQueue.AuthTokensCache.Authorization;
using GameQueue.Core.Services.Managers;
using GameQueue.Host.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Host.Controllers;
[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase, IAuthenticationController
{
    private readonly IUserManager userManager;
    private readonly IJwtService jwtService;
    private readonly RedisAuthTokensCache tokensCache;

    public AuthenticationController(
        IUserManager userManager,
        IJwtService jwtService,
        RedisAuthTokensCache tokensCache)
    {
        this.userManager = userManager;
        this.jwtService = jwtService;
        this.tokensCache = tokensCache;
    }

    [HttpPost("login")]
    public async Task<string> Login(
        [FromQuery(Name = "login")] string username,
        [FromQuery(Name = "password")] string password,
        CancellationToken token = default)
    {
        var user = await userManager.TryLogin(username, password, token);
        if (user == null)
        {
            throw new UnauthorizedException();
        }

        var jwtToken = jwtService.GenerateToken(user);
        await tokensCache.SetKeyValue(user.Id.ToString(), user.Name, token);
        return jwtToken;
    }

    [HttpGet("logout")]
    public async Task Logout(CancellationToken token = default)
    {
        var userId = User.FindFirst(ClaimTypes.Sid) ?? throw new UnauthorizedException();
        var userIdStr = userId.Value;
        await tokensCache.RemoveKey(userIdStr, token);
    }

    [Authorize(Policy = CacheTokenRequirement.Name)]
    [HttpGet]
    public Task<ICollection<string>> Me(CancellationToken token)
        => Task.FromResult(
            (ICollection<string>)User
                .Claims
                .Select(x => x.Value)
                .ToList());
}
