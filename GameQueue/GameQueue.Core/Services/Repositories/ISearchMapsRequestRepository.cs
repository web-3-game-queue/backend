using GameQueue.Core.Entities;

namespace GameQueue.Core.Contracts.Services.Repositories;

public interface ISearchMapsRequestRepository
{
    Task<ICollection<SearchMapsRequest>> GetAllAsync(CancellationToken token = default);

    Task<SearchMapsRequest> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(SearchMapsRequest request, CancellationToken token = default);

    Task ApproveAsync(int id, CancellationToken token = default);

    Task CancelAsync(int id, CancellationToken token = default);

    Task FinishAsync(int id, CancellationToken token = default);

    Task DeleteAsync(int id, CancellationToken token = default);
}
