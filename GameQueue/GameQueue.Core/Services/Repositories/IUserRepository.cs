using GameQueue.Core.Entities;

namespace GameQueue.Core.Services.Repositories;

public interface IUserRepository
{
    Task<ICollection<User>> GetAllAsync(CancellationToken token = default);

    Task<User> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(User user, CancellationToken token = default);

    Task<User> GetByUsername(string username, CancellationToken token = default);
}
