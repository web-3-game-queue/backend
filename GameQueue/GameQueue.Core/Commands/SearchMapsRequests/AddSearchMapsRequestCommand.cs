namespace GameQueue.Core.Commands.SearchMapsRequests;

public sealed record AddSearchMapsRequestCommand
{
    public int SearchMapsRequestId { get; set; }

    public int CreatorUserId { get; set; }

    public int MapId { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;
}
