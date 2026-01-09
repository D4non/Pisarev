using Project.Models.DTO;

namespace Project.Services.Interfaces;

public interface IPlayerService
{
    Task<IEnumerable<PlayerDto>> GetAllAsync();
    Task<PlayerDto?> GetByIdAsync(Guid id);
    Task<PlayerDto> CreateAsync(CreatePlayerDto dto, string userRole);
    Task<PlayerDto> UpdateAsync(Guid id, UpdatePlayerDto dto, string userRole);
    Task<bool> DeleteAsync(Guid id, string userRole);
}

