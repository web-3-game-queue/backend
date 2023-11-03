using GameQueue.Backend.Api.Contracts.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;

namespace GameQueue.Backend.Controllers;
[Route("api/[controller]")]
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
        staticDataUrl = configuration["STATIC_DATA_URL"] ?? throw new NullReferenceException("STATIC_DATA_URL");
        httpClient = httpClientFactory.CreateClient();
        this.minioClient = minioClient;
    }

    [HttpGet("{*urlSuffix}")]
    public async Task GetUrl(
        [FromRoute(Name = "urlSuffix")] string urlSuffix)
    {

        var dataUrl = new Uri(new Uri(staticDataUrl), urlSuffix);
        var data = await httpClient.GetAsync(dataUrl);
        await copyResponseMessageIntoContext(HttpContext, data);
    }

    [HttpGet("test_minio")]
    public async Task TestMinio()
    {
        var bucketsList = await minioClient.ListBucketsAsync();
        foreach (var bucket in bucketsList.Buckets)
        {
            var observable = minioClient.ListObjectsAsync(new ListObjectsArgs().WithBucket(bucket.Name).WithRecursive(true));
            var subscription = observable.Subscribe(
                item => Console.WriteLine($"Object: {item.Key}"),
                ex => Console.WriteLine($"OnError: {ex}"),
                () => Console.WriteLine($"Listed all objects in bucket {bucket.Name}\n"));
        }
        Console.WriteLine(string.Join(", ", bucketsList));
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
