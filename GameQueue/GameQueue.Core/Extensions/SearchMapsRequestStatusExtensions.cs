using GameQueue.Api.Contracts.Models;
using GameQueue.Core.Models;
using GameQueue.DataAccess.Exceptions;
using System.ComponentModel;

namespace GameQueue.Core.Extensions;

public static class SearchMapsRequestStatusExtensions
{
    public static SearchMapsRequestStatusApi ToSearchMapsRequestStatusApi(this SearchMapsRequestStatus status)
        => status switch {
            SearchMapsRequestStatus.Draft => SearchMapsRequestStatusApi.Draft,
            SearchMapsRequestStatus.Composed => SearchMapsRequestStatusApi.Composed,
            SearchMapsRequestStatus.Done => SearchMapsRequestStatusApi.Done,
            SearchMapsRequestStatus.Cancelled => SearchMapsRequestStatusApi.Cancelled,
            SearchMapsRequestStatus.Deleted => SearchMapsRequestStatusApi.Deleted,
            _ => throw new InvalidEnumArgumentException(nameof(status)),
        };

    public static bool CanBeChangedTo(this SearchMapsRequestStatus current, SearchMapsRequestStatus newStatus)
        => (current, newStatus) switch {
            (SearchMapsRequestStatus.Draft, SearchMapsRequestStatus.Composed) => true,
            (SearchMapsRequestStatus.Deleted, SearchMapsRequestStatus.Draft) => true,
            (SearchMapsRequestStatus.Composed, SearchMapsRequestStatus.Done) => true,
            (SearchMapsRequestStatus.Deleted, SearchMapsRequestStatus.Cancelled) => false,
            (_, SearchMapsRequestStatus.Cancelled) => true,
            (_, SearchMapsRequestStatus.Deleted) => true,
            _ => false
        };

    public static void ValidateChangeTo(this SearchMapsRequestStatus status, SearchMapsRequestStatus newStatus) {
        if (!status.CanBeChangedTo(newStatus))
        {
            throw new InvalidSearchMapsRequestStatusUpdateException(status, newStatus);
        }
    }
}
