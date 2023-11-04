using GameQueue.Core.Entities;

namespace GameQueue.Core.Services.Repositories;

public interface IMapRepository
{
    Task<ICollection<Map>> GetAllAsync(CancellationToken token = default);

    Task<Map> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(Map map, CancellationToken token = default);

    Task UpdateAsync(Map map, CancellationToken token = default);

    Task DeleteAsync(Map map, CancellationToken token = default);
}
