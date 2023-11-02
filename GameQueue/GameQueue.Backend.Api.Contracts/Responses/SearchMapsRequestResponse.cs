using GameQueue.Backend.Api.Contracts.Models;

namespace GameQueue.Backend.Api.Contracts.Responses;

public sealed record SearchMapsRequestResponse
{
    public int Id { get; set; }

    public int CreatorUserId { get; set; }

    public SearchMapsRequestStatusApi Status { get; set; } = SearchMapsRequestStatusApi.Draft;

    public DateTime CreationDate { get; set; } = DateTime.Now;
}
