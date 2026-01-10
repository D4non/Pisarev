using Project.Models.DTO;
using Project.Models.Entities;
using Project.Repositories.Interfaces;
using Project.Services.Interfaces;

namespace Project.Services;

public class CoachService : ICoachService
{
    private readonly ICoachRepository _coachRepository;
    private readonly ILogger<CoachService> _logger;

    public CoachService(ICoachRepository coachRepository, ILogger<CoachService> logger)
    {
        _coachRepository = coachRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<CoachDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all coaches");
        var coaches = await _coachRepository.GetAllAsync();
        return coaches.Select(c => new CoachDto
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            DateOfBirth = c.DateOfBirth,
            LicenseType = c.LicenseType
        });
    }

    public async Task<CoachDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting coach by id {Id}", id);
        var coach = await _coachRepository.GetByIdAsync(id);

        if (coach == null)
        {
            _logger.LogWarning("Coach with id {Id} not found", id);
            return null;
        }

        return new CoachDto
        {
            Id = coach.Id,
            FirstName = coach.FirstName,
            LastName = coach.LastName,
            DateOfBirth = coach.DateOfBirth,
            LicenseType = coach.LicenseType
        };
    }

    public async Task<CoachDto> CreateAsync(CreateCoachDto dto, string userRole)
    {
        if (userRole != "Admin" && userRole != "Manager")
        {
            _logger.LogWarning("User with role {Role} attempted to create coach without permission", userRole);
            throw new UnauthorizedAccessException("You do not have permission to create coaches");
        }

        _logger.LogInformation("Creating new coach {FirstName} {LastName}", dto.FirstName, dto.LastName);

        var coach = new Coach
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            LicenseType = dto.LicenseType
        };

        var created = await _coachRepository.CreateAsync(coach);
        _logger.LogInformation("Coach {Id} created successfully", created.Id);

        return new CoachDto
        {
            Id = created.Id,
            FirstName = created.FirstName,
            LastName = created.LastName,
            DateOfBirth = created.DateOfBirth,
            LicenseType = created.LicenseType
        };
    }

    public async Task<CoachDto> UpdateAsync(Guid id, UpdateCoachDto dto, string userRole)
    {
        if (userRole != "Admin" && userRole != "Manager")
        {
            _logger.LogWarning("User with role {Role} attempted to update coach {Id} without permission", userRole, id);
            throw new UnauthorizedAccessException("You do not have permission to update coaches");
        }

        _logger.LogInformation("Updating coach {Id}", id);

        var coach = await _coachRepository.GetByIdAsync(id);
        if (coach == null)
        {
            _logger.LogWarning("Coach with id {Id} not found for update", id);
            throw new KeyNotFoundException($"Coach with id {id} not found");
        }

        coach.FirstName = dto.FirstName;
        coach.LastName = dto.LastName;
        coach.DateOfBirth = dto.DateOfBirth;
        coach.LicenseType = dto.LicenseType;

        var updated = await _coachRepository.UpdateAsync(coach);
        _logger.LogInformation("Coach {Id} updated successfully", updated.Id);

        return new CoachDto
        {
            Id = updated.Id,
            FirstName = updated.FirstName,
            LastName = updated.LastName,
            DateOfBirth = updated.DateOfBirth,
            LicenseType = updated.LicenseType
        };
    }

    public async Task<bool> DeleteAsync(Guid id, string userRole)
    {
        if (userRole != "Admin")
        {
            _logger.LogWarning("User with role {Role} attempted to delete coach {Id} without permission", userRole, id);
            throw new UnauthorizedAccessException("Only admins can delete coaches");
        }

        _logger.LogInformation("Deleting coach {Id}", id);
        var result = await _coachRepository.DeleteAsync(id);

        if (result)
        {
            _logger.LogInformation("Coach {Id} deleted successfully", id);
        }
        else
        {
            _logger.LogWarning("Coach with id {Id} not found for deletion", id);
        }

        return result;
    }
}

