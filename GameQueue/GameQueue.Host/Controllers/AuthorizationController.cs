using GameQueue.Api.Contracts.Controllers;
using GameQueue.Core.Services.Managers;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Host.Controllers;
[Route("api/authorization")]
[ApiController]
public class AuthorizationController : ControllerBase, IAuthorizationController
{
    private readonly IUserManager userManager;

    public AuthorizationController(IUserManager userManager)
    {
        this.userManager = userManager;
    }

    [HttpPost]
    public async Task Register(
        [FromQuery(Name = "name")] string username,
        [FromQuery(Name = "password")] string password,
        CancellationToken token)
    {
        await userManager.Register(username, password, token);
    }
}
