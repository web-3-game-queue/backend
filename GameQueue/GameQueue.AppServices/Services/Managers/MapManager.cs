using GameQueue.Core.Commands.Maps;
using GameQueue.Core.Contracts.Services.Repositories;
using GameQueue.Core.Models;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Services.Managers;
using GameQueue.Core.Services;

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
        await s3Manager.AddObjectAsync(addMapCommand.CoverImageUrl, addMapCommand.CoverImageData, addMapCommand.ContentType, token);
        await mapRepository.AddAsync(map, token);
    }

    public async Task UpdateAsync(UpdateMapCommand updateMapCommand, CancellationToken token = default)
    {
        if (updateMapCommand.FieldsAreEmpty())
        {
            throw new ValidationException("Update command can not be empty");
        }
        var map = await mapRepository.GetByIdAsync(updateMapCommand.Id, token);
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
            CoverImageUrl = addMapCommand.CoverImageUrl,
            Price = addMapCommand.Price
        };

    private void updateMapFromCommand(UpdateMapCommand updateMapCommand, ref Map map)
        => map = map with {
            Name = updateMapCommand.Name ?? map.Name,
            Width = updateMapCommand.Width ?? map.Width,
            Height = updateMapCommand.Height ?? map.Height,
            MaxPlayersCount = updateMapCommand.MaxPlayersCount ?? map.MaxPlayersCount,
            CoverImageUrl = updateMapCommand.CoverImageUrl ?? map.CoverImageUrl,
            Price = updateMapCommand.Price ?? map.Price,
        };
}
