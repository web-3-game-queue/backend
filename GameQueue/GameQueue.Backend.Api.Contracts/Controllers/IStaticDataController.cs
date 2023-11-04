namespace GameQueue.Api.Contracts.Controllers;

public interface IStaticDataController
{
    Task GetUrl(string urlSuffix);
}
