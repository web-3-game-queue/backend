using GameQueue.Api.Contracts.Requests.Users;
using GameQueue.Api.Contracts.Responses;

namespace GameQueue.Api.Contracts.Controllers;

public interface IUserController
{
    Task<ICollection<UserResponse>> GetAllAsync(CancellationToken token = default);

    Task<UserResponse> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(AddUserRequest addUserRequest, CancellationToken token = default);
}
