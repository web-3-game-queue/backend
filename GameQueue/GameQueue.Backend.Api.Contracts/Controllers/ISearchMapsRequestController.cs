using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Core.Backend.Api.Contracts.Requests.Maps;

namespace GameQueue.Backend.Api.Contracts.Controllers;

public interface ISearchMapsRequestController
{
    Task<ICollection<SearchMapsRequestResponse>> GetAll(CancellationToken token = default);

    Task<SearchMapsRequestResponse> GetById(int id, CancellationToken token = default);

    Task Add(AddSearchMapsRequestRequest addSearchMapsRequestCommand, CancellationToken token = default);

    Task Compose(int creatorId, int id, CancellationToken token = default);

    Task Delete(int creatorId, int id, CancellationToken token = default);

    Task Cancel(int moderatorId, int id, CancellationToken token = default);

    Task Finish(int moderatorId, int id, CancellationToken token = default);
}
