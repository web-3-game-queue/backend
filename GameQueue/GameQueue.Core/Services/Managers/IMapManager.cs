using GameQueue.Core.Commands.Maps;
using GameQueue.Core.Models;

namespace GameQueue.Core.Services.Managers;

public interface IMapManager
{
    Task<ICollection<Map>> GetAllAsync(CancellationToken token = default);

    Task<Map> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(AddMapCommand addMapCommand, CancellationToken token = default);

    Task UpdateAsync(UpdateMapCommand updateMapCommand, CancellationToken token = default);

    Task DeleteAsync(int id, CancellationToken token = default);
}
