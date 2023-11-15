using GameQueue.Core.Entities;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Models;
using GameQueue.Core.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameQueue.DataAccess.Repositories;

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

    public async Task DeleteAsync(Map map, CancellationToken token = default)
    {
        db.Maps.Remove(map);
        await db.SaveChangesAsync(token);
    }

    public async Task<ICollection<Map>> GetFiltered(string filterName, int maxPlayersCount, CancellationToken token = default)
        => await db.Maps
            .Where(x =>
                x.Name.Contains(filterName)
                && x.MaxPlayersCount <= maxPlayersCount)
            .OrderBy(x => x.Id)
            .ToListAsync(token);

    private async Task<Map> findOrThrow(int id, CancellationToken token)
        => await db.Maps.FindAsync(id, token)
            ?? throw new EntityNotFoundException(typeof(Map), id);
}
