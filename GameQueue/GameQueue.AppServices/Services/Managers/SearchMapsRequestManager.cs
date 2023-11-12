using GameQueue.Core.Commands.SearchMapsRequests;
using GameQueue.Core.Entities;
using GameQueue.Core.Services.Managers;
using GameQueue.Core.Services.Repositories;

namespace GameQueue.AppServices.Services.Managers;

internal class SearchMapsRequestManager : ISearchMapsRequestManager
{
    private readonly ISearchMapsRequestRepository searchMapsRequestRepository;
    private readonly IUserRepository userRepository;
    private readonly IMapRepository mapRepository;

    public SearchMapsRequestManager(
        ISearchMapsRequestRepository searchMapsRequestRepository,
        IUserRepository userRepository,
        IMapRepository mapRepository)
    {
        this.searchMapsRequestRepository = searchMapsRequestRepository;
        this.userRepository = userRepository;
        this.mapRepository = mapRepository;
    }

    public async Task<ICollection<SearchMapsRequest>> GetAllAsync(
        DateTimeOffset? beginDate,
        DateTimeOffset? endDate,
        string? username,
        CancellationToken token = default)
        => await searchMapsRequestRepository.GetAllAsync(
            beginDate ?? DateTimeOffset.MinValue,
            endDate ?? DateTimeOffset.MaxValue,
            username ?? string.Empty,
            token);

    public async Task<SearchMapsRequest> GetByIdAsync(int id, CancellationToken token = default)
        => await searchMapsRequestRepository.GetByIdAsync(id, token);

    public async Task<SearchMapsRequest> GetByIdAndUserId(int id, int userId, CancellationToken token = default)
        => await searchMapsRequestRepository.GetByIdAndUserId(id, userId, token);

    public async Task<ICollection<SearchMapsRequest>> GetUserRequests(int userId, CancellationToken token = default)
        => await searchMapsRequestRepository.GetUserRequestsAsync(userId, token);

    public async Task<SearchMapsRequest?> GetUserCurrentRequestAsync(int userId, CancellationToken token = default)
        => await searchMapsRequestRepository.GetUserCurrentRequestAsync(userId, token);

    public async Task<SearchMapsRequest> GetOrCreateUserCurrentRequest(int userId, CancellationToken token = default)
        => await searchMapsRequestRepository.GetOrCreateUserCurrentRequestAsync(userId, token);

    public async Task AddAsync(AddSearchMapsRequestCommand addSearchMapsRequestCommand, CancellationToken token = default)
    {
        var user = await userRepository.GetByIdAsync(addSearchMapsRequestCommand.CreatorUserId, token);
        var map = await mapRepository.GetByIdAsync(addSearchMapsRequestCommand.MapId, token);
        var searchMapsRequest = new SearchMapsRequest {
            CreatorUserId = user.Id
        };
        var requestToMap = new RequestToMap {
            MapId = map.Id,
            SearchMapsRequest = searchMapsRequest,
        };
        searchMapsRequest.RequestsToMap.Add(requestToMap);
        await searchMapsRequestRepository.AddAsync(searchMapsRequest, token);
    }

    public async Task AddMap(int searchMapsRequestId, int mapId, CancellationToken token = default)
        => await searchMapsRequestRepository.AddMap(searchMapsRequestId, mapId, token);

    public async Task RemoveMap(int searchMapsRequestId, int mapId, CancellationToken token = default)
        => await searchMapsRequestRepository.RemoveMap(searchMapsRequestId, mapId, token);

    public async Task<int> AddMapToUser(int mapId, int userId, CancellationToken token = default)
    {
        var searchMapsRequest = await searchMapsRequestRepository.GetOrCreateUserCurrentRequestAsync(userId, token);
        await searchMapsRequestRepository.AddMap(searchMapsRequest.Id, mapId, token);
        return searchMapsRequest.Id;
    }

    public async Task RemoveMapFromUser(int mapId, int userId, CancellationToken token = default)
    {
        var searchMapsRequest = await searchMapsRequestRepository.GetOrCreateUserCurrentRequestAsync(userId, token);
        await searchMapsRequestRepository.RemoveMap(searchMapsRequest.Id, mapId, token);
    }

    public async Task ComposeAsync(int clientId, int id, CancellationToken token = default)
    {
        await searchMapsRequestRepository.GetByIdAndUserId(id, clientId, token);
        await searchMapsRequestRepository.ComposeAsync(id, token);
    }

    public async Task DeleteAsync(int clientId, int id, CancellationToken token = default)
    {
        await searchMapsRequestRepository.GetByIdAndUserId(id, clientId, token);
        await searchMapsRequestRepository.DeleteAsync(id, token);
    }

    public async Task CancelAsync(int id, CancellationToken token = default)
    {
        await searchMapsRequestRepository.CancelAsync(id, token);
    }

    public async Task FinishAsync(int id, CancellationToken token = default)
    {
        await searchMapsRequestRepository.FinishAsync(id, token);
    }
}
