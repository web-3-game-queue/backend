namespace GameQueue.Core.Commands.SearchMapsRequests;

public sealed record AddSearchMapsRequestCommand
{
    public int CreatorUserId { get; set; }

    public int MapId { get; set; }

    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;
}
