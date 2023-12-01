using System.Security.Claims;
using GameQueue.Api.Contracts.Controllers;
using GameQueue.Api.Contracts.Exceptions;
using GameQueue.Api.Contracts.Responses;
using GameQueue.AuthTokensCache;
using GameQueue.AuthTokensCache.Authorization;
using GameQueue.Core.Extensions;
using GameQueue.Core.Services.Managers;
using GameQueue.Host.Extensions;
using GameQueue.Host.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
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
        return jwtToken;
    }

    [HttpGet("logout")]
    public async Task Logout(CancellationToken token = default)
    {
        var jwt = await HttpContext.GetTokenAsync("access_token");
        await tokensCache.SetKeyValue(jwt, "1", token);
    }

    [Authorize(Policy = CacheTokenRequirement.Name)]
    [HttpGet]
    public async Task<UserResponse> Me(CancellationToken token)
    {
        var claims = (ICollection<string>)User
                    .Claims
                    .Select(x => x.Value)
                    .ToList();
        var userId = User.Id();
        var user = await userManager.GetByIdAsync(userId, token);
        var userResponse = user.ToUserResponse();
        userResponse.Claims = claims;
        return userResponse;
    }
}
