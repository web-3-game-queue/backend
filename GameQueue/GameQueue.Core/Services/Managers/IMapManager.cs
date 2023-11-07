using GameQueue.Core.Commands.Maps;
using GameQueue.Core.Entities;

namespace GameQueue.Core.Services.Managers;

public interface IMapManager
{
    Task<ICollection<Map>> GetAllAsync(CancellationToken token = default);

    Task<Map> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(AddMapCommand addMapCommand, CancellationToken token = default);

    Task UpdateAsync(UpdateMapCommand updateMapCommand, CancellationToken token = default);

    Task DeleteAsync(int id, CancellationToken token = default);

    Task ForceDeleteAsync(int id, CancellationToken token = default);

    Task<ICollection<Map>> GetFiltered(string filterName, decimal maxPrice, CancellationToken token = default);

    Task MakeAvailable(int id, CancellationToken token = default);
}
