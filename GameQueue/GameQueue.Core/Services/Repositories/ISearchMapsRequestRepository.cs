﻿using GameQueue.Core.Entities;

namespace GameQueue.Core.Services.Repositories;

public interface ISearchMapsRequestRepository
{
    Task<ICollection<SearchMapsRequest>> GetAllAsync(CancellationToken token = default);

    Task<SearchMapsRequest> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(SearchMapsRequest request, CancellationToken token = default);

    Task AddMap(int searchMapsRequestId, int mapId, CancellationToken token = default);

    Task RemoveMap(int searchMapsRequestId, int mapId, CancellationToken token = default);

    Task ComposeAsync(int id, CancellationToken token = default);

    Task DeleteAsync(int id, CancellationToken token = default);

    Task CancelAsync(int id, CancellationToken token = default);

    Task FinishAsync(int id, CancellationToken token = default);
}
