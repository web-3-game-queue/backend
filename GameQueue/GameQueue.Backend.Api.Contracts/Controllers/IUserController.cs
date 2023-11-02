using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Core.Backend.Api.Contracts.Requests.Maps;

namespace GameQueue.Backend.Api.Contracts.Controllers;

public interface IUserController
{
    Task<ICollection<UserResponse>> GetAllAsync(CancellationToken token = default);

    Task<UserResponse> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(AddUserRequest addUserRequest, CancellationToken token = default);
}
