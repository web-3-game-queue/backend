using GameQueue.Api.Contracts.Requests.SearchMapsRequests;
using GameQueue.Api.Contracts.Responses;

namespace GameQueue.Api.Contracts.Controllers;

public interface ISearchMapsRequestController
{
    Task<ICollection<SearchMapsRequestResponse>> GetAll(CancellationToken token = default);

    Task<SearchMapsRequestResponseVerbose> GetById(int id, CancellationToken token = default);

    Task Add(AddSearchMapsRequestRequest addSearchMapsRequestCommand, CancellationToken token = default);

    Task AddMap(int searchMapsRequestId, int mapId, CancellationToken token = default);

    Task RemoveMap(int searchMapsRequestId, int mapId, CancellationToken token = default);

    Task Compose(int creatorId, int id, CancellationToken token = default);

    Task Delete(int creatorId, int id, CancellationToken token = default);

    Task Cancel(int moderatorId, int id, CancellationToken token = default);

    Task Finish(int moderatorId, int id, CancellationToken token = default);
}
