using GameQueue.Core.Services.Repositories;

namespace GameQueue.DataAccess.Repositories;

internal class RequestToMapRepository : IRequestToMapRepository
{
    private readonly GameQueueContext db;

    public RequestToMapRepository(GameQueueContext db)
    {
        this.db = db;
    }

    public async Task RemoveRequestsToMap(int mapId, CancellationToken cancellationToken = default)
    {
        var requestToMapsToRemove = db
            .RequestsToMap
            .Where(x =>
                x.MapId == mapId
                && x.SearchMapsRequest.Status == Core.Models.SearchMapsRequestStatus.Draft);
        db.RequestsToMap.RemoveRange(requestToMapsToRemove);
        await db.SaveChangesAsync();
    }
}
