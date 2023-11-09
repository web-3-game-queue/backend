﻿using System.Data;
using GameQueue.Api.Contracts.Controllers;
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

    [Authorize(Roles = "Administrator, Moderator")]
    [HttpGet]
    public async Task<ICollection<SearchMapsRequestResponse>> GetAll(CancellationToken token = default)
        => (await searchMapsRequestManager.GetAllAsync(token))
            .Select(x => x.ToSearchMapsRequestResponse())
            .ToList();

    [Authorize(Roles = "Administrator, Moderator, Client")]
    [HttpGet("my")]
    public async Task<ICollection<SearchMapsRequestResponse>> GetUserRequests(CancellationToken token)
    {
        var userId = User.Id();
        var searchMapsRequests = await searchMapsRequestManager.GetUserRequests(userId, token);
        return searchMapsRequests
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

    [Authorize(Roles = "Client")]
    [HttpPut("add_map/{map_id:int:min(0)}")]
    public async Task AddMap(
        [FromRoute(Name = "map_id")] int mapId,
        CancellationToken token = default)
    {
        var userId = User.Id();
        await searchMapsRequestManager.AddMapToUser(mapId, userId, token);
    }

    [Authorize(Roles = "Client")]
    [HttpDelete("remove_map/{map_id:int:min(0)}")]
    public async Task RemoveMap(
        [FromRoute(Name = "map_id")] int mapId,
        CancellationToken token = default)
    {
        var userId = User.Id();
        await searchMapsRequestManager.RemoveMapFromUser(mapId, userId, token);
    }

    [Authorize(Roles = "Client")]
    [HttpPut("compose/{id:int:min(0)}")]
    public async Task Compose(
        [FromQuery(Name = "creator_id")] int creatorId,
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await searchMapsRequestManager.ComposeAsync(creatorId, id, token);

    [Authorize(Roles = "Client")]
    [HttpDelete("delete/{id:int:min(0)}")]
    public async Task Delete(
        [FromQuery(Name = "creator_id")] int creatorId,
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await searchMapsRequestManager.DeleteAsync(creatorId, id, token);

    [Authorize(Roles = "Administrator, Moderator")]
    [HttpPut("cancel/{id:int:min(0)}")]
    public async Task Cancel(
        [FromQuery(Name = "moderator_id")] int moderatorId,
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await searchMapsRequestManager.CancelAsync(moderatorId, id, token);

    [Authorize(Roles = "Administrator, Moderator")]
    [HttpPut("finish/{id:int:min(0)}")]
    public async Task Finish(
        [FromQuery(Name = "moderator_id")] int moderatorId,
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await searchMapsRequestManager.FinishAsync(moderatorId, id, token);
}