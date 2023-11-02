using GameQueue.Core.Commands.SearchMapsRequests;
using GameQueue.Core.Models;

namespace GameQueue.Core.Contracts.Services.Managers;

public interface ISearchMapsRequestManager
{
    Task<ICollection<SearchMapsRequest>> GetAllAsync(CancellationToken token = default);

    Task<SearchMapsRequest> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(AddSearchMapsRequestCommand addSearchMapsRequestCommand, CancellationToken token = default);

    Task AddMap(int searchMapsRequestId, int mapId, CancellationToken token = default);

    Task RemoveMap(int searchMapsRequestId, int mapId, CancellationToken token = default);

    Task ComposeAsync(int creatorId, int id, CancellationToken token = default);

    Task DeleteAsync(int creatorId, int id, CancellationToken token = default);

    Task CancelAsync(int moderatorId, int id, CancellationToken token = default);

    Task FinishAsync(int moderatorId, int id, CancellationToken token = default);
}
