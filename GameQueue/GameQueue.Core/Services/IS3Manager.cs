namespace GameQueue.Core.Services;

public interface IS3Manager
{
    public Task AddObjectAsync(string objectUrl, Stream data, CancellationToken token = default);

    public Task DeleteObjectAsync(string objectUrl, CancellationToken token = default);

    public Task<ICollection<string>> GetAllObjectUrlsAsync(bool recursive = true, CancellationToken token = default);
}
