using GameQueue.Backend.Api.Contracts.Controllers;
using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Core.Backend.Api.Contracts.Requests.Maps;
using GameQueue.Core.Commands.Users;
using GameQueue.Core.Contracts.Services.Managers;
using GameQueue.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Backend.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase, IUserController
{
    private IUserManager userManager;

    public UserController(IUserManager userManager)
    {
        this.userManager = userManager;
    }

    [HttpGet]
    public async Task<ICollection<UserResponse>> GetAllAsync(CancellationToken token = default)
        => (await userManager.GetAllAsync(token))
            .Select(x => x.ToUserResponse())
            .ToList();

    [HttpGet("{id:int:min(0)}")]
    public async Task<UserResponse> GetByIdAsync(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => (await userManager.GetByIdAsync(id, token))
                .ToUserResponse();

    [HttpPost]
    public async Task AddAsync(
        [FromBody] AddUserRequest addUserRequest,
        CancellationToken token = default)
            => await userManager.AddAsync(convertAddUserRequest(addUserRequest), token);

    private AddUserCommand convertAddUserRequest(AddUserRequest request)
        => new AddUserCommand {
            Name = request.Name,
            Password = request.Password,
            Level = request.Level
        };
}
