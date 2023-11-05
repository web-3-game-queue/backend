using GameQueue.Core.Commands.Maps;
using GameQueue.Core.Models;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Services.Managers;
using GameQueue.Core.Services;
using GameQueue.Core.Entities;
using GameQueue.Core.Services.Repositories;

namespace GameQueue.AppServices.Services.Managers;

public class MapManager : IMapManager
{
    private IMapRepository mapRepository;
    private IS3Manager s3Manager;

    public MapManager(
        IMapRepository mapRepository,
        IS3Manager s3Manager)
    {
        this.mapRepository = mapRepository;
        this.s3Manager = s3Manager;
    }

    public async Task<ICollection<Map>> GetAllAsync(CancellationToken token = default)
        => await mapRepository.GetAllAsync(token);

    public async Task<Map> GetByIdAsync(int id, CancellationToken token = default)
        => await mapRepository.GetByIdAsync(id, token);

    public async Task AddAsync(AddMapCommand addMapCommand, CancellationToken token = default)
    {
        var map = convertAddCommandToMap(addMapCommand);
        var image = addMapCommand.CoverImageFile;
        await s3Manager.AddObjectAsync(
            image.Url ?? throw new NullReferenceException(nameof(image.Url)),
            image.FileData,
            image.ContentType,
            token);
        await mapRepository.AddAsync(map, token);
    }

    public async Task UpdateAsync(UpdateMapCommand updateMapCommand, CancellationToken token = default)
    {
        if (updateMapCommand.FieldsAreEmpty())
        {
            throw new ValidationException("Update command can not be empty");
        }
        var map = await mapRepository.GetByIdAsync(updateMapCommand.Id, token);
        if (updateMapCommand.CoverImageFile != null)
        {
            var image = updateMapCommand.CoverImageFile;
            await s3Manager.DeleteObjectAsync(map.CoverImageUrl, token);
            await s3Manager.AddObjectAsync(
                image.Url ?? map.CoverImageUrl,
                image.FileData,
                image.ContentType,
                token);
        }
        updateMapFromCommand(updateMapCommand, ref map);
        await mapRepository.UpdateAsync(map, token);
    }

    public async Task DeleteAsync(int id, CancellationToken token = default)
    {
        var map = await mapRepository.GetByIdAsync(id, token);
        map.Status = MapStatus.Deleted;
        await mapRepository.UpdateAsync(map, token);
    }

    public async Task ForceDeleteAsync(int id, CancellationToken token = default)
    {
        Map map;
        try
        {
            map = await mapRepository.GetByIdAsync(id, token);
        }
        catch (EntityNotFoundException)
        {
            return;
        }
        await mapRepository.DeleteAsync(map, token);
        await s3Manager.DeleteObjectAsync(map.CoverImageUrl, token);
    }

    private Map convertAddCommandToMap(AddMapCommand addMapCommand)
        => new Map {
            Name = addMapCommand.Name,
            Width = addMapCommand.Width,
            Height = addMapCommand.Height,
            MaxPlayersCount = addMapCommand.MaxPlayersCount,
            CoverImageUrl = addMapCommand.CoverImageFile.Url ?? throw new NullReferenceException(nameof(addMapCommand.CoverImageFile.Url)),
            Price = addMapCommand.Price
        };

    private void updateMapFromCommand(UpdateMapCommand updateMapCommand, ref Map map)
    {
        map.Name = updateMapCommand.Name ?? map.Name;
        map.Width = updateMapCommand.Width ?? map.Width;
        map.Height = updateMapCommand.Height ?? map.Height;
        map.MaxPlayersCount = updateMapCommand.MaxPlayersCount ?? map.MaxPlayersCount;
        map.CoverImageUrl = updateMapCommand.CoverImageFile?.Url ?? map.CoverImageUrl;
        map.Price = updateMapCommand.Price ?? map.Price;
    }
}
