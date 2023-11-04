using GameQueue.Api.Contracts.Controllers;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;

namespace GameQueue.Host.Controllers;
[Route("api/static_data")]
[ApiController]
public class StaticDataController : ControllerBase, IStaticDataController
{
    private readonly string staticDataUrl;

    private readonly HttpClient httpClient;
    private readonly IMinioClient minioClient;

    public StaticDataController(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        IMinioClient minioClient)
    {
        var minioBucket = configuration["MINIO_BUCKET"] ?? throw new NullReferenceException("MINIO_BUCKET");
        var staticDataHost = configuration["STATIC_DATA_HOST"] ?? throw new NullReferenceException("STATIC_DATA_HOST");

        staticDataUrl = $"{staticDataHost}/{minioBucket}";

        httpClient = httpClientFactory.CreateClient();
        this.minioClient = minioClient;
    }

    [HttpGet("{*urlSuffix}")]
    public async Task GetUrl(
        [FromRoute(Name = "urlSuffix")] string urlSuffix)
    {
        var dataUrl = $"{staticDataUrl}/{urlSuffix}";
        var data = await httpClient.GetAsync(dataUrl);
        await copyResponseMessageIntoContext(HttpContext, data);
    }

    private async Task copyResponseMessageIntoContext(
        HttpContext context,
        HttpResponseMessage responseMessage)
    {
        var response = context.Response;

        response.StatusCode = (int)responseMessage.StatusCode;

        var responseHeaders = responseMessage.Headers;
        if (responseHeaders.TransferEncodingChunked == true &&
            responseHeaders.TransferEncoding.Count == 1)
        {
            responseHeaders.TransferEncoding.Clear();
        }

        foreach (var header in responseHeaders)
        {
            response.Headers.Append(header.Key, header.Value.ToArray());
        }

        if (responseMessage.Content != null)
        {
            var contentHeaders = responseMessage.Content.Headers;

            foreach (var header in contentHeaders)
            {
                response.Headers.Append(header.Key, header.Value.ToArray());
            }

            await responseMessage.Content.CopyToAsync(response.Body);
        }
    }
}
