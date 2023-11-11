using GameQueue.Core.Commands.SearchMapsRequests;
using GameQueue.Core.Entities;

namespace GameQueue.Core.Services.Managers;

public interface ISearchMapsRequestManager
{
    Task<ICollection<SearchMapsRequest>> GetAllAsync(CancellationToken token = default);

    Task<SearchMapsRequest> GetByIdAsync(int id, CancellationToken token = default);

    Task<SearchMapsRequest> GetByIdAndUserId(int id, int userId, CancellationToken token = default);

    Task<ICollection<SearchMapsRequest>> GetUserRequests(int userId, CancellationToken token = default);

    Task AddAsync(AddSearchMapsRequestCommand addSearchMapsRequestCommand, CancellationToken token = default);

    Task AddMapToUser(int mapId, int userId, CancellationToken token = default);

    Task RemoveMapFromUser(int mapId, int userId, CancellationToken token = default);

    Task AddMap(int searchMapsRequestId, int mapId, CancellationToken token = default);

    Task RemoveMap(int searchMapsRequestId, int mapId, CancellationToken token = default);

    Task ComposeAsync(int creatorId, int id, CancellationToken token = default);

    Task DeleteAsync(int creatorId, int id, CancellationToken token = default);

    Task CancelAsync(int id, CancellationToken token = default);

    Task FinishAsync(int id, CancellationToken token = default);
}
