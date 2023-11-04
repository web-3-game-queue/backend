using GameQueue.Core.Commands.SearchMapsRequests;
using GameQueue.Core.Entities;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Models;
using GameQueue.Core.Services.Managers;
using GameQueue.Core.Services.Repositories;
using Microsoft.Extensions.Configuration;

namespace GameQueue.AppServices.Services.Managers;

internal class SearchMapsRequestManager : ISearchMapsRequestManager
{
    private readonly ISearchMapsRequestRepository searchMapsRequestRepository;
    private readonly IUserRepository userRepository;
    private readonly IMapRepository mapRepository;

    private readonly int moderatorId;

    public SearchMapsRequestManager(
        ISearchMapsRequestRepository searchMapsRequestRepository,
        IUserRepository userRepository,
        IMapRepository mapRepository,
        IConfiguration configuration)
    {
        this.searchMapsRequestRepository = searchMapsRequestRepository;
        this.userRepository = userRepository;
        this.mapRepository = mapRepository;
        moderatorId = int.Parse(configuration["ModeratorId"] ?? throw new NullReferenceException("ModeratorId"));
    }

    public async Task<ICollection<SearchMapsRequest>> GetAllAsync(CancellationToken token = default)
        => await searchMapsRequestRepository.GetAllAsync(token);

    public async Task<SearchMapsRequest> GetByIdAsync(int id, CancellationToken token = default)
        => await searchMapsRequestRepository.GetByIdAsync(id, token);

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

    public async Task ComposeAsync(int clientId, int id, CancellationToken token = default)
    {
        var request = await searchMapsRequestRepository.GetByIdAsync(id);
        if (clientId != request.CreatorUserId)
        {
            throw new UnauthorizedException("Client ids do not match");
        }
        await searchMapsRequestRepository.ComposeAsync(id, token);
    }

    public async Task DeleteAsync(int clientId, int id, CancellationToken token = default)
    {
        var request = await searchMapsRequestRepository.GetByIdAsync(id);
        if (clientId != request.CreatorUserId)
        {
            throw new UnauthorizedException("Client ids do not match");
        }
        await searchMapsRequestRepository.DeleteAsync(id, token);
    }

    public async Task CancelAsync(int moderatorId, int id, CancellationToken token = default)
    {
        if (moderatorId != this.moderatorId)
        {
            throw new UnauthorizedException("Invalid moderator id");
        }
        await searchMapsRequestRepository.CancelAsync(id, token);
    }

    public async Task FinishAsync(int moderatorId, int id, CancellationToken token = default)
    {
        if (moderatorId != this.moderatorId)
        {
            throw new UnauthorizedException("Invalid moderator id");
        }
        await searchMapsRequestRepository.FinishAsync(id, token);
    }
}
