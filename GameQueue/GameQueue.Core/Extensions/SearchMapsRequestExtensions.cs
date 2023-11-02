using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Core.Models;

namespace GameQueue.Core.Extensions;

public static class SearchMapsRequestExtensions
{
    public static SearchMapsRequestResponse ToSearchMapsRequestResponse(this SearchMapsRequest searchMapsRequest)
        => new SearchMapsRequestResponse {
            Id = searchMapsRequest.Id,
            CreatorUserId = searchMapsRequest.CreatorUserId,
            Status = searchMapsRequest.Status.ToSearchMapsRequestStatusApi(),
            CreationDate = searchMapsRequest.CreationDate
        };
}
