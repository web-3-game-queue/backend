using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Core.Backend.Api.Contracts.Requests.Maps;

namespace GameQueue.Backend.Api.Contracts.Controllers;

public interface IMapController
{
    Task<ICollection<MapResponse>> GetAll(CancellationToken token = default);

    Task<MapResponse> GetById(int id, CancellationToken token = default);

    Task Add(AddMapRequest addMapRequest, CancellationToken token = default);

    Task AddToSearchMapsRequest(int mapId, int searchMapsRequestId, CancellationToken token = default);

    Task Update(UpdateMapRequest updateMapRequest, CancellationToken token = default);

    Task Delete(int id, CancellationToken token = default);
}
