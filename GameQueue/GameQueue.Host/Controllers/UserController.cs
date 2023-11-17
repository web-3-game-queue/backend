using GameQueue.Api.Contracts.Controllers;
using GameQueue.Api.Contracts.Requests.Users;
using GameQueue.Api.Contracts.Responses;
using GameQueue.Core.Commands.Users;
using GameQueue.Core.Extensions;
using GameQueue.Core.Services.Managers;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Host.Controllers;
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

    [HttpPut("{id:int:min(0)}")]
    public Task SetLevel(
        [FromRoute(Name = "id")] int userId,
        [FromQuery(Name = "level")] int level,
        CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    private AddUserCommand convertAddUserRequest(AddUserRequest request)
        => new AddUserCommand {
            Name = request.Name,
            Password = request.Password,
            Level = request.Level,
            Role = request.Role.ToUserRole()
        };
}
