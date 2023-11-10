﻿namespace GameQueue.Api.Contracts.Controllers;

public interface IAuthenticationController
{
    public Task<string> Login(string username, string password, CancellationToken token);

    public Task Logout(CancellationToken token);

    public Task<ICollection<string>> Me(CancellationToken token);
}
