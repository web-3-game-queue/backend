using GameQueue.Api.Contracts.Requests.Maps;
using GameQueue.Api.Contracts.Responses;
using Microsoft.AspNetCore.Http;

namespace GameQueue.Api.Contracts.Controllers;

public interface IMapController
{
    Task<ICollection<MapResponse>> GetAll(CancellationToken token = default);

    Task<MapResponse> GetById(int id, CancellationToken token = default);

    Task Add(AddMapRequest addMapRequest, IFormFile coverImageFile, CancellationToken token = default);

    Task Update(int id, UpdateMapRequest updateMapRequest, IFormFile? coverImageFile, CancellationToken token = default);

    Task Delete(int id, CancellationToken token = default);

    Task ForceDelete(int id, CancellationToken token = default);
}
