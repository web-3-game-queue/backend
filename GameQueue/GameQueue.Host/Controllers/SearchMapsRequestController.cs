using GameQueue.Api.Contracts.Controllers;
using GameQueue.Api.Contracts.Responses;
using GameQueue.Core.Extensions;
using GameQueue.Core.Services.Managers;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Host.Controllers;
[Route("api/search_maps_request")]
[ApiController]
public class SearchMapsRequestController : ControllerBase, ISearchMapsRequestController
{
    private readonly ISearchMapsRequestManager searchMapsRequestManager;

    public SearchMapsRequestController(ISearchMapsRequestManager searchMapsRequestManager)
        => this.searchMapsRequestManager = searchMapsRequestManager;

    [HttpGet]
    public async Task<ICollection<SearchMapsRequestResponse>> GetAll(CancellationToken token = default)
        => (await searchMapsRequestManager.GetAllAsync(token))
            .Select(x => x.ToSearchMapsRequestResponse())
            .ToList();

    [HttpGet("{id:int:min(0)}")]
    public async Task<SearchMapsRequestResponseVerbose> GetById(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => (await searchMapsRequestManager.GetByIdAsync(id, token))
                .ToSerchMapsRequestResponseVerbose();

    [HttpPut("add_map/{map_id:int:min(0)}/{search_maps_request_id:int:min(0)}")]
    public async Task AddMap(
        [FromRoute(Name = "map_id")] int mapId,
        [FromRoute(Name = "search_maps_request_id")] int searchMapsRequestId,
        CancellationToken token = default)
            => await searchMapsRequestManager.AddMap(searchMapsRequestId, mapId, token);

    [HttpDelete("remove_map/{map_id:int:min(0)}/{search_maps_request_id:int:min(0)}")]
    public async Task RemoveMap(
        [FromRoute(Name = "map_id")] int mapId,
        [FromRoute(Name = "search_maps_request_id")] int searchMapsRequestId,
        CancellationToken token = default)
            => await searchMapsRequestManager.RemoveMap(searchMapsRequestId, mapId, token);

    [HttpPut("compose/{id:int:min(0)}")]
    public async Task Compose(
        [FromQuery(Name = "creator_id")] int creatorId,
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await searchMapsRequestManager.ComposeAsync(creatorId, id, token);

    [HttpDelete("delete/{id:int:min(0)}")]
    public async Task Delete(
        [FromQuery(Name = "creator_id")] int creatorId,
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await searchMapsRequestManager.DeleteAsync(creatorId, id, token);

    [HttpPut("cancel/{id:int:min(0)}")]
    public async Task Cancel(
        [FromQuery(Name = "moderator_id")] int moderatorId,
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await searchMapsRequestManager.CancelAsync(moderatorId, id, token);

    [HttpPut("finish/{id:int:min(0)}")]
    public async Task Finish(
        [FromQuery(Name = "moderator_id")] int moderatorId,
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await searchMapsRequestManager.FinishAsync(moderatorId, id, token);
}
