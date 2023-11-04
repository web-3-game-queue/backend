using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Core.Backend.Api.Contracts.Requests.Maps;
using Microsoft.AspNetCore.Http;

namespace GameQueue.Backend.Api.Contracts.Controllers;

public interface IMapController
{
    Task<ICollection<MapResponse>> GetAll(CancellationToken token = default);

    Task<MapResponse> GetById(int id, CancellationToken token = default);

    Task Add(AddMapRequest addMapRequest, IFormFile coverImageFile, CancellationToken token = default);

    Task Update(int id, UpdateMapRequest updateMapRequest, CancellationToken token = default);

    Task Delete(int id, CancellationToken token = default);

    Task ForceDelete(int id, CancellationToken token = default);
}
