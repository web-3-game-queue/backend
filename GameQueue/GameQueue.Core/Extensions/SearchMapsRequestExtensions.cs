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

    public static SearchMapsRequestResponseVerbose ToSerchMapsRequestResponseVerbose(this SearchMapsRequest searchMapsRequest)
        => new SearchMapsRequestResponseVerbose {
            Id = searchMapsRequest.Id,
            CreatorUserId = searchMapsRequest.CreatorUserId,
            Status = searchMapsRequest.Status.ToSearchMapsRequestStatusApi(),
            CreationDate = searchMapsRequest.CreationDate,
            CreatorUser = searchMapsRequest.CreatorUser.ToUserResponse(),
            Maps = searchMapsRequest.RequestsToMap.Select(x => x.Map.ToMapResponse()).ToList()
        };
}
