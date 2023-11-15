namespace GameQueue.Api.Contracts.Requests.Maps;

public record GetFilteredMapsRequest
{
    public string FilterName { get; set; } = null!;
    
    public decimal MaxPrice { get; set; }
}
