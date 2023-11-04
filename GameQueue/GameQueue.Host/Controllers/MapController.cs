using GameQueue.Api.Contracts.Controllers;
using GameQueue.Api.Contracts.Requests.Maps;
using GameQueue.Api.Contracts.Responses;
using GameQueue.Core.Commands.Maps;
using GameQueue.Core.Extensions;
using GameQueue.Core.Models;
using GameQueue.Core.Services.Managers;
using GameQueue.Host.Exceptions;
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
        [FromForm] AddMapRequest addMapRequest,
        IFormFile coverImageFile,
        CancellationToken token = default)
    {
        if (!AllowedContentTypes.Contains(coverImageFile.ContentType))
        {
            throw new InvalidContentTypeException(coverImageFile.ContentType, AllowedContentTypes);
        }
        await mapManager.AddAsync(convertAddMapRequest(addMapRequest, coverImageFile), token);
    }

    [HttpPut("{id:int:min(0)}")]
    public async Task Update(
        [FromRoute(Name = "id")] int id,
        [FromForm] UpdateMapRequest updateMapRequest,
        IFormFile? coverImageFile,
        CancellationToken token = default)
            => await mapManager
                .UpdateAsync(convertUpdateMapRequest(id, updateMapRequest, coverImageFile), token);

    [HttpDelete("{id:int:min(0)}")]
    public async Task Delete(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await mapManager.DeleteAsync(id, token);

    [HttpDelete("force_delete/{id:int:min(0)}")]
    public async Task ForceDelete(
        [FromRoute(Name = "id")] int id,
        CancellationToken token = default)
            => await mapManager.ForceDeleteAsync(id, token);

    private AddMapCommand convertAddMapRequest(AddMapRequest addMapRequest, IFormFile coverImageFile)
        => new AddMapCommand {
            Name = addMapRequest.Name,
            Width = addMapRequest.Width,
            Height = addMapRequest.Height,
            MaxPlayersCount = addMapRequest.MaxPlayersCount,
            Price = addMapRequest.Price,
            CoverImageFile = new CoverImageUploadModel {
                Url = addMapRequest.CoverImageUrl,
                FileData = coverImageFile.OpenReadStream(),
                ContentType = coverImageFile.ContentType
            }
        };

    private UpdateMapCommand convertUpdateMapRequest(int id, UpdateMapRequest updateMapRequest, IFormFile? coverImageFile)
        => new UpdateMapCommand {
            Id = id,
            Name = updateMapRequest.Name,
            Width = updateMapRequest.Width,
            Height = updateMapRequest.Height,
            MaxPlayersCount = updateMapRequest.MaxPlayersCount,
            Price = updateMapRequest.Price,
            CoverImageFile = coverImageFile == null ? null : new CoverImageUploadModel {
                Url = updateMapRequest.CoverImageUrl,
                FileData = coverImageFile.OpenReadStream(),
                ContentType = coverImageFile.ContentType
            }
        };
}
