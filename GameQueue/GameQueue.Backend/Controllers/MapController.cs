using GameQueue.Backend.Api.Contracts.Controllers;
using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Core.Backend.Api.Contracts.Requests.Maps;
using GameQueue.Core.Commands.Maps;
using GameQueue.Core.Extensions;
using GameQueue.Core.Services.Managers;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Backend.Controllers;
[Route("api/map")]
[ApiController]
public class MapController : ControllerBase, IMapController
{
    private readonly IMapManager mapManager;

    public MapController(IMapManager mapManager) => this.mapManager = mapManager;

    [HttpGet]
    public async Task<ICollection<MapResponse>> GetAll(CancellationToken token = default)
        => (await mapManager.GetAllAsync(token))
                .Select(x => x.ToMapResponse())
                .ToList();

    [HttpGet("{id:int:min(0)}")]
    public async Task<MapResponse> GetById(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => (await mapManager.GetByIdAsync(id, token))
                .ToMapResponse();

    [HttpPost]
    public async Task Add(
        [FromBody] AddMapRequest addMapRequest,
        CancellationToken token = default)
            => await mapManager
                .AddAsync(convertAddMapRequest(addMapRequest), token);

    [HttpPost("{map_id:int:min(0)}/add_to_request/{search_maps_request_id:int:min(0)}")]
    public async Task AddToSearchMapsRequest(
        [FromRoute(Name = "map_id")] int mapId,
        [FromRoute(Name = "search_maps_request_id")] int searchMapsRequestId,
        CancellationToken token = default)
            => await mapManager
                .AddToSearchMapsRequestAsync(mapId, searchMapsRequestId, token);

    [HttpPut("{id:int:min(0)}")]
    public async Task Update(
        [FromRoute(Name = "id")] int id,
        [FromBody] UpdateMapRequest updateMapRequest,
        CancellationToken token = default)
            => await mapManager
                .UpdateAsync(convertUpdateMapRequest(id, updateMapRequest), token);

    [HttpDelete("{id:int:min(0)}")]
    public async Task Delete(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
        => await mapManager.DeleteAsync(id, token);

    private AddMapCommand convertAddMapRequest(AddMapRequest addMapRequest)
        => new AddMapCommand {
            Name = addMapRequest.Name,
            Width = addMapRequest.Width,
            Height = addMapRequest.Height,
            MaxPlayersCount = addMapRequest.MaxPlayersCount,
            CoverImageUrl = addMapRequest.CoverImageUrl,
            Price = addMapRequest.Price
        };

    private UpdateMapCommand convertUpdateMapRequest(int id, UpdateMapRequest updateMapRequest)
        => new UpdateMapCommand {
            Id = id,
            Name = updateMapRequest.Name,
            Width = updateMapRequest.Width,
            Height = updateMapRequest.Height,
            MaxPlayersCount = updateMapRequest.MaxPlayersCount,
            CoverImageUrl = updateMapRequest.CoverImageUrl,
            Price = updateMapRequest.Price
        };
}
