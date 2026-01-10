using Project.Models.Entities;

namespace Project.Repositories.Interfaces;

public interface IMatchRepository
{
    Task<IEnumerable<Match>> GetAllAsync();
    Task<Match?> GetByIdAsync(Guid id);
    Task<Match> CreateAsync(Match match);
    Task<Match> UpdateAsync(Match match);
    Task<bool> DeleteAsync(Guid id);
}

