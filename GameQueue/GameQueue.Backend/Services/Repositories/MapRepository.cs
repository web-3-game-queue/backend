using GameQueue.Backend.DataAccess;
using GameQueue.Core.Contracts.Services.Repositories;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GameQueue.Backend.Services.Repositories;

internal class MapRepository : IMapRepository
{
    private readonly GameQueueContext db;

    public MapRepository(GameQueueContext db) => this.db = db;

    public async Task<ICollection<Map>> GetAllAsync(CancellationToken token = default)
        => await db.Maps.ToListAsync(token);

    public async Task<Map> GetByIdAsync(int id, CancellationToken token = default)
        => await findOrThrow(id, token);

    public async Task AddAsync(Map map, CancellationToken token = default)
    {
        await db.Maps.AddAsync(map, token);
        await db.SaveChangesAsync(token);
    }

    public async Task UpdateAsync(Map map, CancellationToken token = default)
    {
        db.Maps.Update(map);
        await db.SaveChangesAsync(token);
    }

    public async Task DeleteAsync(int id, CancellationToken token = default)
    {
        var map = await findOrThrow(id, token);
        map.Status = MapStatus.Deleted;
        map.RequestsToMap.Clear();
        await db.SaveChangesAsync(token);
    }

    public async Task AddToSearchRequest(int mapId, int requestId, CancellationToken token = default)
    {
        var request = await db.SearchMapsRequests.FindAsync(requestId)
            ?? throw new EntityNotFoundException(typeof(SearchMapsRequest), mapId);
        var map = await db.Maps.FindAsync(mapId)
            ?? throw new EntityNotFoundException(typeof(Map), mapId);
        await db.RequestsToMap.AddAsync(new RequestToMap {
            SearchMapsRequestId = request.Id,
            MapId = map.Id
        }, token);
    }

    private async Task<Map> findOrThrow(int id, CancellationToken token)
        => await db.Maps.FindAsync(id, token)
            ?? throw new EntityNotFoundException(typeof(Map), id);
}
