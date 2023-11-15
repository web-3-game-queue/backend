using System.Reactive.Linq;
using GameQueue.Core.Services;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace GameQueue.S3Access;

internal class S3Manager : IS3Manager
{
    private readonly string minioBucket;

    private readonly IMinioClient minioClient;

    public S3Manager(
        IConfiguration configuration,
        IMinioClient minioClient)
    {
        minioBucket = configuration["MINIO_BUCKET"] ?? throw new NullReferenceException("MINIO_BUCKET");

        this.minioClient = minioClient;
    }

    public async Task AddObjectAsync(string objectUrl, Stream data, string? contentType, CancellationToken token = default)
    {
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(minioBucket)
            .WithObject(objectUrl)
            .WithStreamData(data)
            .WithObjectSize(data.Length);
        if (contentType != null)
        {
            putObjectArgs = putObjectArgs.WithContentType(contentType);
        }
        await minioClient.PutObjectAsync(putObjectArgs, token);
    }

    public async Task DeleteObjectAsync(string objectUrl, CancellationToken token = default)
    {
        var deleteObjectArgs = new RemoveObjectArgs()
            .WithBucket(minioBucket)
            .WithObject(objectUrl);
        await minioClient.RemoveObjectAsync(deleteObjectArgs, token);
    }

    public async Task<ICollection<string>> GetAllObjectUrlsAsync(bool recursive = true, CancellationToken token = default)
    {
        var listObjectsArgs = new ListObjectsArgs()
            .WithBucket(minioBucket)
            .WithRecursive(recursive);
        var observable = minioClient.ListObjectsAsync(listObjectsArgs, token);
        return await observable.Select(x => x.Key).ToList();
    }
}
