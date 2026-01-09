using Project.Models.Entities;

namespace Project.Repositories.Interfaces;

public interface ICoachRepository
{
    Task<IEnumerable<Coach>> GetAllAsync();
    Task<Coach?> GetByIdAsync(Guid id);
    Task<Coach> CreateAsync(Coach coach);
    Task<Coach> UpdateAsync(Coach coach);
    Task<bool> DeleteAsync(Guid id);
}

