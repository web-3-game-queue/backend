using GameQueue.Backend.Api.Contracts.Controllers;
using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Core.Backend.Api.Contracts.Requests.Maps;
using GameQueue.Core.Commands.Users;
using GameQueue.Core.Contracts.Services.Managers;
using GameQueue.Core.Models;
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

    public async Task<ICollection<UserResponse>> GetAllAsync(CancellationToken token = default)
        => (await userManager.GetAllAsync(token))
            .Select(convertUser)
            .ToList();

    public async Task<UserResponse> GetByIdAsync(int id, CancellationToken token = default)
        => convertUser(await userManager.GetByIdAsync(id, token));

    public async Task AddAsync(AddUserRequest addUserRequest, CancellationToken token = default)
        => await userManager.AddAsync(convertAddUserRequest(addUserRequest), token);

    private UserResponse convertUser(User user)
        => new UserResponse {
            Id = user.Id,
            Name = user.Name,
            Level = user.Level,
        };

    private AddUserCommand convertAddUserRequest(AddUserRequest request)
        => new AddUserCommand {
            Name = request.Name,
            Password = request.Password,
            Level = request.Level
        };
}
