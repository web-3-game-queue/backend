namespace GameQueue.Backend.Api.Contracts.Controllers;

public interface IStaticDataController
{
    Task GetUrl(string urlSuffix);
}
