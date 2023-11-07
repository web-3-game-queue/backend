using GameQueue.Api.Contracts.Models;
using GameQueue.Core.Models;

namespace GameQueue.Core.Extensions;

public static class MapStatusExtensions
{
    public static MapStatusApi ToMapStatusApi(this MapStatus mapStatus)
        => mapStatus switch {
            MapStatus.Pending => MapStatusApi.Pending,
            MapStatus.Available => MapStatusApi.Available,
            MapStatus.Deleted => MapStatusApi.Deleted,
            _ => throw new NotImplementedException(),
        };
}
