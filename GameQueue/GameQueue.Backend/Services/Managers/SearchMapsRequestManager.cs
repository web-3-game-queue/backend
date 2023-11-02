using GameQueue.Core.Commands.SearchMapsRequests;
using GameQueue.Core.Contracts.Services.Managers;
using GameQueue.Core.Contracts.Services.Repositories;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Models;

namespace GameQueue.Backend.Services.Managers;

internal class SearchMapsRequestManager : ISearchMapsRequestManager
{
    private readonly ISearchMapsRequestRepository searchMapsRequestRepository;

    private readonly ModeratorUser moderatorUser;

    public SearchMapsRequestManager(
        ISearchMapsRequestRepository searchMapsRequestRepository,
        ModeratorUser moderatorUser)
    {
        this.searchMapsRequestRepository = searchMapsRequestRepository;
        this.moderatorUser = moderatorUser;
    }

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
        if (moderatorId != moderatorUser.Id)
        {
            throw new UnauthorizedException("Invalid moderator id");
        }
        await searchMapsRequestRepository.CancelAsync(id, token);
    }

    public async Task FinishAsync(int moderatorId, int id, CancellationToken token = default)
    {
        if (moderatorId != moderatorUser.Id)
        {
            throw new UnauthorizedException("Invalid moderator id");
        }
        await searchMapsRequestRepository.FinishAsync(id, token);
    }
}
