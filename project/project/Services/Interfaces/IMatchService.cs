using Project.Models.DTO;

namespace Project.Services.Interfaces;

public interface IMatchService
{
    Task<IEnumerable<MatchDto>> GetAllAsync();
    Task<MatchDto?> GetByIdAsync(Guid id);
    Task<MatchDto> CreateAsync(CreateMatchDto dto, string userRole);
    Task<MatchDto> UpdateAsync(Guid id, UpdateMatchDto dto, string userRole);
    Task<bool> DeleteAsync(Guid id, string userRole);
}

