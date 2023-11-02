using System.ComponentModel;
using GameQueue.Backend.Api.Contracts.Controllers;
using GameQueue.Backend.Api.Contracts.Models;
using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Core.Backend.Api.Contracts.Requests.Maps;
using GameQueue.Core.Commands.SearchMapsRequests;
using GameQueue.Core.Contracts.Services.Managers;
using GameQueue.Core.Extensions;
using GameQueue.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Backend.Controllers;
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

    [HttpPost]
    public async Task Add(
        [FromBody] AddSearchMapsRequestRequest addSearchMapsRequestCommand,
        CancellationToken token = default)
            => await searchMapsRequestManager.AddAsync(convertAddSearchMapsRequestRequest(addSearchMapsRequestCommand), token);

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

    private AddSearchMapsRequestCommand convertAddSearchMapsRequestRequest(AddSearchMapsRequestRequest request)
        => new AddSearchMapsRequestCommand {
            CreatorUserId = request.CreatorUserId,
            MapId = request.MapId,
            CreationDate = request.CreationDate
        };
}
