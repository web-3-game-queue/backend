using GameQueue.Core.Commands.Maps;
using GameQueue.Core.Contracts.Services.Repositories;
using GameQueue.Core.Entities;
using GameQueue.Core.Services.Managers;

namespace GameQueue.Backend.Services.Managers;

public class MapManager : IMapManager
{
    private IMapRepository mapRepository;

    public MapManager(IMapRepository mapRepository) => this.mapRepository = mapRepository;

    public async Task<ICollection<Map>> GetAllAsync(CancellationToken token = default)
        => await mapRepository.GetAllAsync(token);

    public async Task<Map> GetByIdAsync(int id, CancellationToken token = default)
        => await mapRepository.GetByIdAsync(id, token);

    public async Task AddAsync(AddMapCommand addMapCommand, CancellationToken token = default)
    {
        var map = convertAddCommandToMap(addMapCommand);
        await mapRepository.AddAsync(map, token);
    }

    public async Task AddToSearchMapsRequest(int mapId, int searchMapsRequestId, CancellationToken token = default)
    {
        var map = await mapRepository.GetByIdAsync(mapId, token);
        var requestToMap = new RequestToMap {
            Map = map,
            SearchMapsRequestId = searchMapsRequestId
        };
        map.RequestsToMap.Add(requestToMap);
        await mapRepository.UpdateAsync(map, token);
    }

    public async Task UpdateAsync(UpdateMapCommand updateMapCommand, CancellationToken token = default)
    {
        var map = await mapRepository.GetByIdAsync(updateMapCommand.Id, token);
        updateMapCommand.Update(ref map);
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
}
