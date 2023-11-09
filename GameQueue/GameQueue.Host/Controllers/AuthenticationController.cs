using System.Security.Claims;
using GameQueue.Api.Contracts.Controllers;
using GameQueue.Api.Contracts.Exceptions;
using GameQueue.Core.Services.Managers;
using GameQueue.Host.Extensions;
using GameQueue.Host.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Host.Controllers;
[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase, IAuthenticationController
{
    private readonly IUserManager userManager;
    private readonly IJwtService jwtService;

    public AuthenticationController(
        IUserManager userManager,
        IJwtService jwtService)
    {
        this.userManager = userManager;
        this.jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task Login(
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

        var claims = user.ToClaimsList();
        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }

    [HttpGet("logout")]
    public async Task Logout(CancellationToken token = default)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet]
    public Task<ICollection<string>> Me(CancellationToken token)
        => Task.FromResult(
            (ICollection<string>)User
                .Claims
                .Select(x => x.Value)
                .ToList());
}
