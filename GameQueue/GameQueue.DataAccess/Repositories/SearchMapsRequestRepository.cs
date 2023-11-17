using System.Data;
using GameQueue.Core.Entities;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Extensions;
using GameQueue.Core.Models;
using GameQueue.Core.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameQueue.DataAccess.Repositories;

internal class SearchMapsRequestRepository : ISearchMapsRequestRepository
{
    private readonly GameQueueContext db;

    public SearchMapsRequestRepository(GameQueueContext db) => this.db = db;

    public async Task<ICollection<SearchMapsRequest>> GetAllAsync(
        DateTimeOffset beginDate,
        DateTimeOffset endDate,
        string username,
        CancellationToken token = default)
    {

        var query = db.SearchMapsRequests
                .Include(x => x.RequestsToMap)
                .Where(x =>
                    x.CreationDate >= beginDate
                    && x.CreationDate <= endDate
                    && x.CreatorUser.Name.Contains(username));
        return await query.ToListAsync(token);
    }

    public async Task<SearchMapsRequest> GetByIdAsync(int id, CancellationToken token = default)
    {
        var searchMapsRequest = await db.SearchMapsRequests
            .Include(x => x.RequestsToMap)
            .ThenInclude(y => y.Map)
            .Include(x => x.CreatorUser)
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(token)
            ?? throw new EntityNotFoundException(typeof(SearchMapsRequest), id);
        return searchMapsRequest;
    }

    public async Task<SearchMapsRequest> GetByIdAndUserId(int id, int userId, CancellationToken token = default)
    {
        var searchMapsRequest = await db
            .SearchMapsRequests
            .Include(x => x.RequestsToMap)
            .ThenInclude(y => y.Map)
            .Include(x => x.CreatorUser)
            .Where(x =>
                x.Id == id
                && x.CreatorUserId == userId)
            .SingleOrDefaultAsync(token)
            ?? throw new EntityNotFoundException(typeof(SearchMapsRequest), id);
        return searchMapsRequest;
    }

    public async Task<ICollection<SearchMapsRequest>> GetUserRequestsAsync(int userId, CancellationToken token = default)
        => await db
            .SearchMapsRequests
            .Include(x => x.RequestsToMap)
            .Where(x => x.CreatorUserId == userId && x.Status != SearchMapsRequestStatus.Deleted)
            .ToListAsync();

    public async Task<SearchMapsRequest?> GetUserCurrentRequestAsync(int userId, CancellationToken token = default)
        => await db
            .SearchMapsRequests
            .Include(x => x.RequestsToMap)
            .ThenInclude(y => y.Map)
            .Include(x => x.CreatorUser)
            .Where(x => x.CreatorUserId == userId
                    && x.Status == SearchMapsRequestStatus.Draft)
            .SingleOrDefaultAsync(token);

    public async Task<SearchMapsRequest> GetOrCreateUserCurrentRequestAsync(int userId, CancellationToken token = default)
    {
        var user = await db.Users.FindAsync(userId);
        var searchMapsRequest = await db
            .SearchMapsRequests
            .Include(x => x.RequestsToMap)
            .ThenInclude(y => y.Map)
            .Include(x => x.CreatorUser)
            .Where(x => x.CreatorUserId == userId
                    && x.Status == SearchMapsRequestStatus.Draft)
            .SingleOrDefaultAsync(token);
        if (searchMapsRequest == null)
        {
            var newSearchMapsRequest = new SearchMapsRequest { CreatorUserId = userId };
            db.SearchMapsRequests.Add(newSearchMapsRequest);
            await db.SaveChangesAsync(token);
            newSearchMapsRequest.CreatorUser = user ?? throw new NullReferenceException(nameof(user));
            return newSearchMapsRequest;
        }
        return searchMapsRequest;
    }

    public async Task AddAsync(SearchMapsRequest SearchMapsRequest, CancellationToken token = default)
    {
        db.SearchMapsRequests.Add(SearchMapsRequest);
        await db.SaveChangesAsync(token);
    }

    public async Task AddMap(int searchMapsRequestId, int mapId, CancellationToken token = default)
    {
        var searchMapsRequest = await findOrThrow(searchMapsRequestId, token);
        var map = await db.Maps.FindAsync(mapId, token)
            ?? throw new EntityNotFoundException(typeof(Map), mapId);
        await db.RequestsToMap.AddAsync(new RequestToMap {
            SearchMapsRequestId = searchMapsRequest.Id,
            MapId = map.Id
        }, token);
        await db.SaveChangesAsync();
    }

    public async Task RemoveMap(int searchMapsRequestId, int mapId, CancellationToken token = default)
    {
        var searchMapsRequest = await db.SearchMapsRequests
            .Include(x => x.RequestsToMap)
            .Where(x => x.Id == searchMapsRequestId)
            .SingleOrDefaultAsync()
            ?? throw new EntityNotFoundException(typeof(SearchMapsRequest), searchMapsRequestId);
        var map = await db.Maps.FindAsync(mapId, token);
        if (map == null)
        {
            return;
        }
        var requestToMap = searchMapsRequest
            .RequestsToMap
            .Where(y => y.MapId == mapId)
            .First();
        db.RequestsToMap.Remove(requestToMap);
        if (searchMapsRequest.RequestsToMap.Count <= 1)
        {
            db.SearchMapsRequests.Remove(searchMapsRequest);
        }
        await db.SaveChangesAsync(token);
    }

    public async Task ComposeAsync(int id, CancellationToken token = default)
    {
        var searchMapsRequest = await findOrThrow(id, token);
        searchMapsRequest.Status.ValidateChangeTo(SearchMapsRequestStatus.Composed);
        searchMapsRequest.Status = SearchMapsRequestStatus.Composed;
        searchMapsRequest.ComposeDate = DateTimeOffset.UtcNow;
        db.Update(searchMapsRequest);
        await db.SaveChangesAsync(token);
    }

    public async Task DeleteAsync(int id, CancellationToken token = default)
    {
        var searchMapsRequest = await findOrThrow(id, token);
        searchMapsRequest.Status.ValidateChangeTo(SearchMapsRequestStatus.Deleted);
        searchMapsRequest.Status = SearchMapsRequestStatus.Deleted;
        db.Update(searchMapsRequest);
        await db.SaveChangesAsync(token);
    }

    public async Task CancelAsync(int handlerUserId, int id, CancellationToken token = default)
    {
        var searchMapsRequest = await findOrThrow(id, token);
        searchMapsRequest.Status.ValidateChangeTo(SearchMapsRequestStatus.Cancelled);
        searchMapsRequest.Status = SearchMapsRequestStatus.Cancelled;
        searchMapsRequest.HandledByUserId = handlerUserId;
        db.Update(searchMapsRequest);
        await db.SaveChangesAsync(token);
    }

    public async Task FinishAsync(int handlerUserId, int id, CancellationToken token = default)
    {
        var searchMapsRequest = await findOrThrow(id, token);
        searchMapsRequest.Status.ValidateChangeTo(SearchMapsRequestStatus.Done);
        searchMapsRequest.Status = SearchMapsRequestStatus.Done;
        searchMapsRequest.DoneDate = DateTimeOffset.UtcNow;
        searchMapsRequest.HandledByUserId = handlerUserId;
        db.Update(searchMapsRequest);
        await db.SaveChangesAsync(token);
    }

    private async Task<SearchMapsRequest> findOrThrow(int id, CancellationToken token)
        => await db.SearchMapsRequests.FindAsync(id, token)
            ?? throw new EntityNotFoundException(typeof(SearchMapsRequest), id);
}
