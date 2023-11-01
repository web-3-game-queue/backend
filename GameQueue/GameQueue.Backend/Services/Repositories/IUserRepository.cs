using GameQueue.Core.Entities;

namespace GameQueue.Backend.Services.Repositories;

public interface IUserRepository
{
    Task<ICollection<User>> GetAllAsync(CancellationToken token = default);

    Task<User> GetByIdAsync(int id, CancellationToken token = default);
}
