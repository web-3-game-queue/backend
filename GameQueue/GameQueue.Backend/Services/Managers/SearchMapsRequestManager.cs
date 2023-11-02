using GameQueue.Core.Commands.SearchMapsRequests;
using GameQueue.Core.Contracts.Services.Managers;
using GameQueue.Core.Contracts.Services.Repositories;
using GameQueue.Core.Entities;

namespace GameQueue.Backend.Services.Managers;

internal class SearchMapsRequestManager : ISearchMapsRequestManager
{
    private readonly ISearchMapsRequestRepository searchMapsRequestRepository;

    public SearchMapsRequestManager(ISearchMapsRequestRepository searchMapsRequestRepository)
        => this.searchMapsRequestRepository = searchMapsRequestRepository;

    public async Task<ICollection<SearchMapsRequest>> GetAllAsync(CancellationToken token = default)
        => await searchMapsRequestRepository.GetAllAsync(token);

    public async Task<SearchMapsRequest> GetByIdAsync(int id, CancellationToken token = default)
        => await searchMapsRequestRepository.GetByIdAsync(id, token);

    public async Task AddAsync(AddSearchMapsRequestCommand addSearchMapsRequestCommand, CancellationToken token = default)
    {
        var searchMapsRequest = new SearchMapsRequest {
            CreatorUserId = addSearchMapsRequestCommand.CreatorUserId
        };
        var requestToMap = new RequestToMap {
            MapId = addSearchMapsRequestCommand.MapId,
            SearchMapsRequest = searchMapsRequest,
        };
        searchMapsRequest.RequestsToMap.Add(requestToMap);
        await searchMapsRequestRepository.AddAsync(searchMapsRequest, token);
    }

    public async Task ApproveAsync(int id, CancellationToken token = default)
        => await searchMapsRequestRepository.ApproveAsync(id, token);

    public async Task CancelAsync(int id, CancellationToken token = default)
        => await searchMapsRequestRepository.CancelAsync(id, token);

    public async Task FinishAsync(int id, CancellationToken token = default)
        => await searchMapsRequestRepository.FinishAsync(id, token);

    public async Task DeleteAsync(int id, CancellationToken token = default)
        => await searchMapsRequestRepository.DeleteAsync(id, token);
}
