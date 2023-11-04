using GameQueue.Api.Contracts.Responses;
using GameQueue.Core.Entities;

namespace GameQueue.Core.Extensions;

public static class MapExtensions
{
    public static MapResponse ToMapResponse(this Map map)
        => new MapResponse {
            Id = map.Id,
            Name = map.Name,
            Width = map.Width,
            Height = map.Height,
            MaxPlayersCount = map.MaxPlayersCount,
            CoverImageUrl = map.CoverImageUrl,
            Price = map.Price
        };
}
