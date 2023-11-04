using GameQueue.Api.Contracts.Models;
using GameQueue.Core.Models;
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
}
