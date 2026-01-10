using Project.Models.Entities;

namespace Project.Repositories.Interfaces;

public interface IPlayerRepository
{
    Task<IEnumerable<Player>> GetAllAsync();
    Task<Player?> GetByIdAsync(Guid id);
    Task<Player> CreateAsync(Player player);
    Task<Player> UpdateAsync(Player player);
    Task<bool> DeleteAsync(Guid id);
}

