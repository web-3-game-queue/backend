using System.Security.Claims;
using GameQueue.Api.Contracts.Controllers;
using GameQueue.Api.Contracts.Exceptions;
using GameQueue.Core.Services.Managers;
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

    public AuthenticationController(IUserManager userManager)
    {
        this.userManager = userManager;
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

        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
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
