using Microsoft.Extensions.Caching.Distributed;
using Project.Models.DTO;
using Project.Models.Entities;
using Project.Repositories.Interfaces;
using Project.Services.Interfaces;

namespace Project.Services;

public class ClubService : IClubService
{
    private readonly IClubRepository _clubRepository;
    private readonly ILogger<ClubService> _logger;
    private const string CacheKey = "clubs:list";

    public ClubService(IClubRepository clubRepository, ILogger<ClubService> logger)
    {
        _clubRepository = clubRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ClubDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all clubs");
        var clubs = await _clubRepository.GetAllAsync();
        return clubs.Select(c => new ClubDto
        {
            Id = c.Id,
            Name = c.Name,
            Type = c.Type.ToString(),
            City = c.City,
            Country = c.Country,
            Founded = c.Founded
        });
    }

    public async Task<ClubListDto> GetAllAsync(int page, int pageSize, string? search)
    {
        _logger.LogInformation("Getting clubs with page {Page}, pageSize {PageSize}, search {Search}", page, pageSize, search);

        var (items, total) = await _clubRepository.GetPagedAsync(page, pageSize, search);

        var clubs = items.Select(c => new ClubDto
        {
            Id = c.Id,
            Name = c.Name,
            Type = c.Type.ToString(),
            City = c.City,
            Country = c.Country,
            Founded = c.Founded
        }).ToList();

        return new ClubListDto
        {
            Items = clubs,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ClubDto?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Getting club by id {Id}", id);
            var club = await _clubRepository.GetByIdAsync(id);

            if (club == null)
            {
                _logger.LogWarning("Club with id {Id} not found", id);
                return null;
            }

            return new ClubDto
            {
                Id = club.Id,
                Name = club.Name,
                Type = club.Type.ToString(),
                City = club.City,
                Country = club.Country,
                Founded = club.Founded
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting club by id {Id}", id);
            throw;
        }
    }

    public async Task<ClubDto> CreateAsync(CreateClubDto dto, string userRole)
    {
        if (userRole != "Admin" && userRole != "Manager")
        {
            _logger.LogWarning("User with role {Role} attempted to create club without permission", userRole);
            throw new UnauthorizedAccessException("You do not have permission to create clubs");
        }

        _logger.LogInformation("Creating new club {Name}", dto.Name);

        var club = new Club
        {
            Name = dto.Name,
            Type = (ClubType)dto.Type,
            City = dto.City,
            Country = dto.Country,
            Founded = dto.Founded
        };

        var created = await _clubRepository.CreateAsync(club);
        _logger.LogInformation("Club {Id} created successfully", created.Id);

        return new ClubDto
        {
            Id = created.Id,
            Name = created.Name,
            Type = created.Type.ToString(),
            City = created.City,
            Country = created.Country,
            Founded = created.Founded
        };
    }

    public async Task<ClubDto> UpdateAsync(int id, UpdateClubDto dto, string userRole)
    {
        try
        {
            if (userRole != "Admin" && userRole != "Manager")
            {
                _logger.LogWarning("User with role {Role} attempted to update club {Id} without permission", userRole, id);
                throw new UnauthorizedAccessException("You do not have permission to update clubs");
            }

            _logger.LogInformation("Updating club {Id}", id);

            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null)
            {
                _logger.LogWarning("Club with id {Id} not found for update", id);
                throw new KeyNotFoundException($"Club with id {id} not found");
            }

            club.Name = dto.Name;
            club.Type = (ClubType)dto.Type;
            club.City = dto.City;
            club.Country = dto.Country;
            club.Founded = dto.Founded;

            var updated = await _clubRepository.UpdateAsync(club);
            _logger.LogInformation("Club {Id} updated successfully", updated.Id);

            return new ClubDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Type = updated.Type.ToString(),
                City = updated.City,
                Country = updated.Country,
                Founded = updated.Founded
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating club {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id, string userRole)
    {
        if (userRole != "Admin")
        {
            _logger.LogWarning("User with role {Role} attempted to delete club {Id} without permission", userRole, id);
            throw new UnauthorizedAccessException("Only admins can delete clubs");
        }

        _logger.LogInformation("Deleting club {Id}", id);
        var result = await _clubRepository.DeleteAsync(id);

        if (result)
        {
            _logger.LogInformation("Club {Id} deleted successfully", id);
        }
        else
        {
            _logger.LogWarning("Club with id {Id} not found for deletion", id);
        }

        return result;
    }
}

