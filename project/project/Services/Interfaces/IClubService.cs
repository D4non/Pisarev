using Project.Models.DTO;

namespace Project.Services.Interfaces;

public interface IClubService
{
    Task<IEnumerable<ClubDto>> GetAllAsync();
    Task<ClubListDto> GetAllAsync(int page, int pageSize, string? search);
    Task<ClubDto?> GetByIdAsync(int id);
    Task<ClubDto> CreateAsync(CreateClubDto dto, string userRole);
    Task<ClubDto> UpdateAsync(int id, UpdateClubDto dto, string userRole);
    Task<bool> DeleteAsync(int id, string userRole);
}

