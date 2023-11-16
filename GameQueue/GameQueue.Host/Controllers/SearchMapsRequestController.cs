using System.Data;
using GameQueue.Api.Contracts.Controllers;
using GameQueue.Api.Contracts.Requests.SearchMapsRequest;
using GameQueue.Api.Contracts.Responses;
using GameQueue.Core.Entities;
using GameQueue.Core.Extensions;
using GameQueue.Core.Services.Managers;
using GameQueue.Host.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Host.Controllers;
[Route("api/search_maps_request")]
[ApiController]
public class SearchMapsRequestController : ControllerBase, ISearchMapsRequestController
{
    private readonly ISearchMapsRequestManager searchMapsRequestManager;

    public SearchMapsRequestController(ISearchMapsRequestManager searchMapsRequestManager)
        => this.searchMapsRequestManager = searchMapsRequestManager;

    [Authorize(Roles = "Administrator, Moderator, Client")]
    [HttpGet]
    public async Task<ICollection<SearchMapsRequestResponse>> GetUserRequests(CancellationToken token)
    {
        var userId = User.Id();
        var searchMapsRequests = await searchMapsRequestManager.GetUserRequests(userId, token);
        return searchMapsRequests
            .Select(x => x.ToSearchMapsRequestResponse())
            .ToList();
    }

    [Authorize(Roles = "Administrator, Moderator")]
    [HttpGet("all")]
    public async Task<ICollection<SearchMapsRequestResponse>> GetAll(
        [FromQuery(Name = "begin_date")] DateTimeOffset? beginDate,
        [FromQuery(Name = "end_date")] DateTimeOffset? endDate,
        [FromQuery(Name = "username")] string? username,
        CancellationToken token = default)
    {
        var searchMapsRequest = await searchMapsRequestManager.GetAllAsync(
            beginDate,
            endDate,
            username,
            token);
        return searchMapsRequest
                .Select(x => x.ToSearchMapsRequestResponse())
                .ToList();
    }

    [Authorize(Roles = "Administrator, Moderator, Client")]
    [HttpGet("{id:int:min(0)}")]
    public async Task<SearchMapsRequestResponseVerbose> GetById(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
    {
        SearchMapsRequest searchMapsRequest;
        if (User.IsInRole("Administrator") || User.IsInRole("Moderator"))
        {
            searchMapsRequest = await searchMapsRequestManager.GetByIdAsync(id, token);
        }
        else
        {
            var userId = User.Id();
            searchMapsRequest = await searchMapsRequestManager.GetByIdAndUserId(id, userId, token);
        }
        return searchMapsRequest.ToSerchMapsRequestResponseVerbose();
    }

    [Authorize]
    [HttpGet("current")]
    public async Task<SearchMapsRequestResponseVerbose?> GetCurrentSearchMapsRequest(CancellationToken token = default)
    {
        var userId = User.Id();
        var searchMapsRequest = await searchMapsRequestManager.GetUserCurrentRequestAsync(userId, token);
        return searchMapsRequest?.ToSerchMapsRequestResponseVerbose();
    }

    [Authorize]
    [HttpPut("add_map/{map_id:int:min(0)}")]
    public async Task<int> AddMap(
        [FromRoute(Name = "map_id")] int mapId,
        CancellationToken token = default)
    {
        var userId = User.Id();
        var requestId = await searchMapsRequestManager.AddMapToUser(mapId, userId, token);
        return requestId;
    }

    [Authorize]
    [HttpDelete("remove_map/{map_id:int:min(0)}")]
    public async Task RemoveMap(
        [FromRoute(Name = "map_id")] int mapId,
        CancellationToken token = default)
    {
        var userId = User.Id();
        await searchMapsRequestManager.RemoveMapFromUser(mapId, userId, token);
    }

    [Authorize]
    [HttpPut("compose/{id:int:min(0)}")]
    public async Task Compose(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
    {
        await searchMapsRequestManager.ComposeAsync(User.Id(), id, token);
    }

    [Authorize]
    [HttpDelete("delete/{id:int:min(0)}")]
    public async Task Delete(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
    {
        await searchMapsRequestManager.DeleteAsync(User.Id(), id, token);
    }

    [Authorize(Roles = "Administrator, Moderator")]
    [HttpPut("status/{id:int:min(0)}")]
    public Task SetStatus(int id, SetStatusRequest status, CancellationToken token = default)
    {
        return status switch {
            SetStatusRequest.Finished => searchMapsRequestManager.FinishAsync(User.Id(), id, token),
            SetStatusRequest.Cancelled => searchMapsRequestManager.CancelAsync(User.Id(), id, token),
            _ => throw new NotImplementedException()
        };
    }
}
