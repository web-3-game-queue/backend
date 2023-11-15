using GameQueue.Api.Contracts.Responses;

namespace GameQueue.Api.Contracts.Controllers;

public interface ISearchMapsRequestController
{
    Task<ICollection<SearchMapsRequestResponse>> GetAll(
        DateTimeOffset? beginDate,
        DateTimeOffset? endDate,
        string? username,
        CancellationToken token = default);

    Task<ICollection<SearchMapsRequestResponse>> GetUserRequests(CancellationToken token);

    Task<SearchMapsRequestResponseVerbose> GetById(int id, CancellationToken token = default);

    Task<SearchMapsRequestResponseVerbose?> GetCurrentSearchMapsRequest(CancellationToken token = default);

    Task<int> AddMap(int mapId, CancellationToken token = default);

    Task RemoveMap(int mapId, CancellationToken token = default);

    Task Compose(int id, CancellationToken token = default);

    Task Delete(int id, CancellationToken token = default);

    Task Cancel(int id, CancellationToken token = default);

    Task Finish(int id, CancellationToken token = default);
}
