namespace GameQueue.Core.Services.Repositories;

public interface IRequestToMapRepository
{
    Task RemoveRequestsToMap(int mapId, CancellationToken cancellationToken = default);
}
