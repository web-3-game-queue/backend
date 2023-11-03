using GameQueue.Core.Commands.Maps;
using GameQueue.Core.Contracts.Services.Repositories;
using GameQueue.Core.Models;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Services.Managers;

namespace GameQueue.AppServices.Services.Managers;

public class MapManager : IMapManager
{
    private IMapRepository mapRepository;
    private ISearchMapsRequestRepository searchMapsRequestRepository;

    public MapManager(IMapRepository mapRepository, ISearchMapsRequestRepository searchMapsRequestRepository)
    {
        this.mapRepository = mapRepository;
        this.searchMapsRequestRepository = searchMapsRequestRepository;
    }

    public async Task<ICollection<Map>> GetAllAsync(CancellationToken token = default)
        => await mapRepository.GetAllAsync(token);

    public async Task<Map> GetByIdAsync(int id, CancellationToken token = default)
        => await mapRepository.GetByIdAsync(id, token);

    public async Task AddAsync(AddMapCommand addMapCommand, CancellationToken token = default)
    {
        var map = convertAddCommandToMap(addMapCommand);
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
        => await mapRepository.DeleteAsync(id, token);

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
