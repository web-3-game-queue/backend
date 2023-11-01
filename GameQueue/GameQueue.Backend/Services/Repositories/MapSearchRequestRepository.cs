using GameQueue.Backend.DataAccess;
using GameQueue.Core.Contracts.Services.Repositories;
using GameQueue.Core.Contracts.Services.Repositories.Exceptions;
using GameQueue.Core.Entities;
using GameQueue.Core.Entities.MapSearchRequests.Status;
using Microsoft.EntityFrameworkCore;

namespace GameQueue.Backend.Services.Repositories;

internal class MapSearchRequestRepository : IMapSearchRequestRepository
{
    private readonly GameQueueContext db;

    public MapSearchRequestRepository(GameQueueContext db) => this.db = db;

    public async Task<ICollection<MapSearchRequest>> GetAllAsync(CancellationToken token = default)
        => await db.MapSearchRequests.ToListAsync(token);

    public async Task<MapSearchRequest> GetByIdAsync(int id, CancellationToken token = default)
        => await findOrThrow(id, token);

    public async Task AddAsync(MapSearchRequest mapSearchRequest, CancellationToken token = default)
    {
        await db.MapSearchRequests.AddAsync(mapSearchRequest, token);
        await db.SaveChangesAsync();
    }

    public async Task Approve(int id, CancellationToken token = default)
        => await updateStatus(id, MapSearchRequestStatus.InProgress);

    public async Task CancelAsync(int id, CancellationToken token = default)
        => await updateStatus(id, MapSearchRequestStatus.Cancelled, token);

    public async Task Delete(int id, CancellationToken token = default)
        => await updateStatus(id, MapSearchRequestStatus.Deleted, token);

    public async Task Finish(int id, CancellationToken token = default)
        => await updateStatus(id, MapSearchRequestStatus.Done, token);

    private async Task updateStatus(int id, MapSearchRequestStatus status, CancellationToken token = default)
    {
        var mapSearchRequest = await findOrThrow(id, token);
        db.Update(mapSearchRequest);
        mapSearchRequest.Status = status;
        await db.SaveChangesAsync();
    }

    private async Task<MapSearchRequest> findOrThrow(int id, CancellationToken token)
        => await db.MapSearchRequests.FindAsync(id, token)
            ?? throw new EntityNotFound(typeof(MapSearchRequest), id);
}
