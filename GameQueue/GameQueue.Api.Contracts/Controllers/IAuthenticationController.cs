using GameQueue.Api.Contracts.Responses;

namespace GameQueue.Api.Contracts.Controllers;

public interface IAuthenticationController
{
    public Task<string> Login(string username, string password, CancellationToken token);

    public Task Logout(CancellationToken token);

    public Task<UserResponse> Me(CancellationToken token);
}
