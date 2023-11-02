using GameQueue.Backend.DataAccess;
using GameQueue.Core.Contracts.Services.Repositories;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GameQueue.Backend.Services.Repositories;

internal class SearchMapsRequestRepository : ISearchMapsRequestRepository
{
    private readonly GameQueueContext db;

    public SearchMapsRequestRepository(GameQueueContext db) => this.db = db;

    public async Task<ICollection<SearchMapsRequest>> GetAllAsync(CancellationToken token = default)
        => await db.SearchMapsRequests.ToListAsync(token);

    public async Task<SearchMapsRequest> GetByIdAsync(int id, CancellationToken token = default)
    {
        var searchMapsRequest = await db.SearchMapsRequests
            .Include(x => x.RequestsToMap)
            .ThenInclude(y => y.Map)
            .Include(x => x.CreatorUser)
            .Where(x => x.Id == id)
            .FirstAsync()
            ?? throw new EntityNotFoundException(typeof(SearchMapsRequest), id);
        return searchMapsRequest;
    }

    public async Task AddAsync(SearchMapsRequest SearchMapsRequest, CancellationToken token = default)
    {
        await db.SearchMapsRequests.AddAsync(SearchMapsRequest, token);
        await db.SaveChangesAsync();
    }

    public async Task ComposeAsync(int id, CancellationToken token = default)
        => await updateStatus(id, SearchMapsRequestStatus.Composed);

    public async Task DeleteAsync(int id, CancellationToken token = default)
        => await updateStatus(id, SearchMapsRequestStatus.Deleted, token);

    public async Task CancelAsync(int id, CancellationToken token = default)
        => await updateStatus(id, SearchMapsRequestStatus.Cancelled, token);

    public async Task FinishAsync(int id, CancellationToken token = default)
        => await updateStatus(id, SearchMapsRequestStatus.Done, token);

    private async Task updateStatus(int id, SearchMapsRequestStatus status, CancellationToken token = default)
    {
        var SearchMapsRequest = await findOrThrow(id, token);
        db.Update(SearchMapsRequest);
        SearchMapsRequest.Status = status;
        await db.SaveChangesAsync();
    }

    private async Task<SearchMapsRequest> findOrThrow(int id, CancellationToken token)
        => await db.SearchMapsRequests.FindAsync(id, token)
            ?? throw new EntityNotFoundException(typeof(SearchMapsRequest), id);
}
