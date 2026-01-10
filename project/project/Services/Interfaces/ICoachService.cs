using Project.Models.DTO;

namespace Project.Services.Interfaces;

public interface ICoachService
{
    Task<IEnumerable<CoachDto>> GetAllAsync();
    Task<CoachDto?> GetByIdAsync(Guid id);
    Task<CoachDto> CreateAsync(CreateCoachDto dto, string userRole);
    Task<CoachDto> UpdateAsync(Guid id, UpdateCoachDto dto, string userRole);
    Task<bool> DeleteAsync(Guid id, string userRole);
}

