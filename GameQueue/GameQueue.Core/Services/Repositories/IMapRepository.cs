using GameQueue.Core.Models;

namespace GameQueue.Core.Contracts.Services.Repositories;

public interface IMapRepository
{
    Task<ICollection<Map>> GetAllAsync(CancellationToken token = default);

    Task<Map> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(Map map, CancellationToken token = default);

    Task UpdateAsync(Map map, CancellationToken token = default);

    Task DeleteAsync(int id, CancellationToken token = default);

    Task AddToSearchRequest(int mapId, int requestId, CancellationToken token = default);
}
