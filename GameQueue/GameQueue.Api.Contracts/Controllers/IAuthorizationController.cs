namespace GameQueue.Api.Contracts.Controllers;

public interface IAuthorizationController
{
    public Task Register(string username, string password, CancellationToken token);
}
