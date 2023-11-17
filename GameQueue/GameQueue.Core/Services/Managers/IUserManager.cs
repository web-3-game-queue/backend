using GameQueue.Core.Commands.Users;
using GameQueue.Core.Entities;

namespace GameQueue.Core.Services.Managers;

public interface IUserManager
{
    Task<ICollection<User>> GetAllAsync(CancellationToken token = default);

    Task<User> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(AddUserCommand addUserCommand, CancellationToken token = default);

    Task<User?> TryLogin(string username, string password, CancellationToken token = default);

    Task Register(string username, string password, CancellationToken token = default);

    Task SetLevel(int userId, int level, CancellationToken token = default);
}
