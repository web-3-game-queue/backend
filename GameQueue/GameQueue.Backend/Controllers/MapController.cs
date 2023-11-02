using GameQueue.Backend.Api.Contracts.Controllers;
using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Backend.Services.Managers;
using GameQueue.Core.Backend.Api.Contracts.Requests.Maps;
using GameQueue.Core.Commands.Maps;
using GameQueue.Core.Models;
using GameQueue.Core.Services.Managers;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Backend.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MapController : ControllerBase, IMapController
{
    private readonly IMapManager mapManager;

    public MapController(IMapManager mapManager) => this.mapManager = mapManager;

    [HttpGet]
    public async Task<ICollection<MapResponse>> GetAll(CancellationToken token = default)
        => (await mapManager.GetAllAsync(token))
                .Select(convertMap)
                .ToList();

    [HttpGet("{id:int:min(0)}")]
    public async Task<MapResponse> GetById(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => convertMap(await mapManager.GetByIdAsync(id, token));

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

    public async Task Delete(int id, CancellationToken token = default)
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

    private MapResponse convertMap(Map map)
        => new MapResponse {
            Id = map.Id,
            Name = map.Name,
            Width = map.Width,
            Height = map.Height,
            MaxPlayersCount = map.MaxPlayersCount,
            CoverImageUrl = map.CoverImageUrl,
            Price = map.Price
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
