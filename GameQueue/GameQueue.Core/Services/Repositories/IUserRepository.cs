using GameQueue.Core.Models;

namespace GameQueue.Core.Contracts.Services.Repositories;

public interface IUserRepository
{
    Task<ICollection<User>> GetAllAsync(CancellationToken token = default);

    Task<User> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(User user, CancellationToken token = default);
}
