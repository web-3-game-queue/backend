using GameQueue.Api.Contracts.Controllers;
using GameQueue.Api.Contracts.Exceptions;
using GameQueue.Api.Contracts.Requests.Maps;
using GameQueue.Api.Contracts.Responses;
using GameQueue.Core.Commands.Maps;
using GameQueue.Core.Extensions;
using GameQueue.Core.Models;
using GameQueue.Core.Services.Managers;
using GameQueue.Host.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Host.Controllers;
[Route("api/map")]
[ApiController]
public class MapController : ControllerBase, IMapController
{
    private readonly IMapManager mapManager;

    private static readonly string[] AllowedContentTypes = {
        "image/gif",
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    public MapController(IMapManager mapManager) => this.mapManager = mapManager;

    [HttpGet]
    public async Task<ICollection<MapResponse>> GetFiltered(
        [FromQuery] string? filterName,
        [FromQuery] int? maxPlayersCount,
        CancellationToken token = default)
    {
        var maps = await mapManager.GetFiltered(
            filterName ?? string.Empty,
            maxPlayersCount ?? int.MaxValue,
            token);
        return maps.Select(x => x.ToMapResponse()).ToList();
    }

    [HttpGet("all")]
    public async Task<ICollection<MapResponse>> GetAll(CancellationToken token = default)
    {
        ICollection<Core.Entities.Map> maps = await mapManager.GetAllAsync(token);
        return maps
                .Select(x => x.ToMapResponse())
                .ToList();
    }

    [HttpGet("{id:int:min(0)}")]
    public async Task<MapResponse> GetById(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
    {
        var map = await mapManager.GetByIdAsync(id, token);
        return map.ToMapResponse();
    }

    [HttpPost]
    public async Task Add(
        [FromForm] AddMapRequest addMapRequest,
        IFormFile? coverImageFile,
        CancellationToken token = default)
    {
        if (coverImageFile != null && !AllowedContentTypes.Contains(coverImageFile.ContentType))
        {
            throw new InvalidContentTypeException(coverImageFile.ContentType, AllowedContentTypes);
        }
        await mapManager.AddAsync(convertAddMapRequest(addMapRequest, coverImageFile), token);
    }

    //[Authorize(Roles = "Administrator")]
    [HttpPut("{id:int:min(0)}")]
    public async Task Update(
        [FromRoute(Name = "id")] int id,
        [FromForm] UpdateMapRequest updateMapRequest,
        IFormFile? coverImageFile,
        CancellationToken token = default)
    {
        await mapManager.UpdateAsync(convertUpdateMapRequest(id, updateMapRequest, coverImageFile), token);
    }

    [HttpDelete("delete/{id:int:min(0)}")]
    public async Task Delete(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await mapManager.DeleteAsync(id, token);

    [HttpDelete("force_delete/{id:int:min(0)}")]
    public async Task ForceDelete(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await mapManager.ForceDeleteAsync(id, token);

    [HttpPut("make_available/{id:int:min(0)}")]
    public async Task MakeAvailable(
        int id,
        CancellationToken token = default)
        => await mapManager.MakeAvailable(id);

    private AddMapCommand convertAddMapRequest(AddMapRequest addMapRequest, IFormFile? coverImageFile)
        => new AddMapCommand {
            Name = addMapRequest.Name,
            Width = addMapRequest.Width,
            Height = addMapRequest.Height,
            MaxPlayersCount = addMapRequest.MaxPlayersCount,
            CoverImageFile =
                coverImageFile == null ? null
                : new CoverImageUploadModel {
                    Url = addMapRequest.CoverImageUrl,
                    FileData = coverImageFile.OpenReadStream(),
                    ContentType = coverImageFile.ContentType
                },
            Description = addMapRequest.Description
        };

    private UpdateMapCommand convertUpdateMapRequest(int id, UpdateMapRequest updateMapRequest, IFormFile? coverImageFile)
        => new UpdateMapCommand {
            Id = id,
            Name = updateMapRequest.Name,
            Width = updateMapRequest.Width,
            Height = updateMapRequest.Height,
            MaxPlayersCount = updateMapRequest.MaxPlayersCount,
            CoverImageFile = coverImageFile == null ? null : new CoverImageUploadModel {
                Url = updateMapRequest.CoverImageUrl,
                FileData = coverImageFile.OpenReadStream(),
                ContentType = coverImageFile.ContentType
            },
            Description = updateMapRequest.Description
        };
}
