using GameQueue.Core.Entities;

namespace GameQueue.Core.Contracts.Services.Repositories;

public interface IMapSearchRequestRepository
{
    Task<ICollection<MapSearchRequest>> GetAllAsync(CancellationToken token = default);

    Task<MapSearchRequest> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(MapSearchRequest request, CancellationToken token = default);

    Task Approve(int id, CancellationToken token = default);

    Task CancelAsync(int id, CancellationToken token = default);

    Task Finish(int id, CancellationToken token = default);

    Task Delete(int id, CancellationToken token = default);
}
