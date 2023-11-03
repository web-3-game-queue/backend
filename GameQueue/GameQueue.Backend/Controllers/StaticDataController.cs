using GameQueue.Backend.Api.Contracts.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;

namespace GameQueue.Backend.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StaticDataController : ControllerBase, IStaticDataController
{
    private readonly string minioUrl;
    private readonly HttpClient httpClient;
    private readonly IMinioClient minioClient;

    public StaticDataController(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        IMinioClient minioClient)
    {
        minioUrl = configuration["MINIO_ENDPOINT"] ?? throw new NullReferenceException("MINIO_ENDPOINT");
        httpClient = httpClientFactory.CreateClient();
        this.minioClient = minioClient;
    }

    [HttpGet("{*urlSuffix}")]
    public async Task GetUrl(
        [FromRoute(Name = "urlSuffix")] string urlSuffix)
    {
        var dataUrl = new Uri(new Uri(minioUrl), urlSuffix);
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
