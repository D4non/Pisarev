using Project.Models.Entities;

namespace Project.Repositories.Interfaces;

public interface IClubRepository
{
    Task<IEnumerable<Club>> GetAllAsync();
    Task<Club?> GetByIdAsync(int id);
    Task<Club> CreateAsync(Club club);
    Task<Club> UpdateAsync(Club club);
    Task<bool> DeleteAsync(int id);
    Task<(IEnumerable<Club> items, int total)> GetPagedAsync(int page, int pageSize, string? search);
}

