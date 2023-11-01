using GameQueue.Core.Entities;

namespace GameQueue.Backend.Services.Repositories;

public interface IMapRepository
{
    Task<ICollection<Map>> GetAllAsync(CancellationToken token = default);

    Task<Map> GetByIdAsync(CancellationToken token = default);

    //Task AddAsync()
}
