using Project.Models.DTO;
using Project.Models.Entities;
using Project.Repositories.Interfaces;
using Project.Services.Interfaces;

namespace Project.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(IPlayerRepository playerRepository, ILogger<PlayerService> logger)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<PlayerDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all players");
        var players = await _playerRepository.GetAllAsync();
        return players.Select(p => new PlayerDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Position = p.Position,
            JerseyNumber = p.JerseyNumber
        });
    }

    public async Task<PlayerDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting player by id {Id}", id);
        var player = await _playerRepository.GetByIdAsync(id);

        if (player == null)
        {
            _logger.LogWarning("Player with id {Id} not found", id);
            return null;
        }

        return new PlayerDto
        {
            Id = player.Id,
            FirstName = player.FirstName,
            LastName = player.LastName,
            DateOfBirth = player.DateOfBirth,
            Position = player.Position,
            JerseyNumber = player.JerseyNumber
        };
    }

    public async Task<PlayerDto> CreateAsync(CreatePlayerDto dto, string userRole)
    {
        if (userRole != "Admin" && userRole != "Manager")
        {
            _logger.LogWarning("User with role {Role} attempted to create player without permission", userRole);
            throw new UnauthorizedAccessException("You do not have permission to create players");
        }

        _logger.LogInformation("Creating new player {FirstName} {LastName}", dto.FirstName, dto.LastName);

        var player = new Player
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Position = dto.Position,
            JerseyNumber = dto.JerseyNumber
        };

        var created = await _playerRepository.CreateAsync(player);
        _logger.LogInformation("Player {Id} created successfully", created.Id);

        return new PlayerDto
        {
            Id = created.Id,
            FirstName = created.FirstName,
            LastName = created.LastName,
            DateOfBirth = created.DateOfBirth,
            Position = created.Position,
            JerseyNumber = created.JerseyNumber
        };
    }

    public async Task<PlayerDto> UpdateAsync(Guid id, UpdatePlayerDto dto, string userRole)
    {
        if (userRole != "Admin" && userRole != "Manager")
        {
            _logger.LogWarning("User with role {Role} attempted to update player {Id} without permission", userRole, id);
            throw new UnauthorizedAccessException("You do not have permission to update players");
        }

        _logger.LogInformation("Updating player {Id}", id);

        var player = await _playerRepository.GetByIdAsync(id);
        if (player == null)
        {
            _logger.LogWarning("Player with id {Id} not found for update", id);
            throw new KeyNotFoundException($"Player with id {id} not found");
        }

        player.FirstName = dto.FirstName;
        player.LastName = dto.LastName;
        player.DateOfBirth = dto.DateOfBirth;
        player.Position = dto.Position;
        player.JerseyNumber = dto.JerseyNumber;

        var updated = await _playerRepository.UpdateAsync(player);
        _logger.LogInformation("Player {Id} updated successfully", updated.Id);

        return new PlayerDto
        {
            Id = updated.Id,
            FirstName = updated.FirstName,
            LastName = updated.LastName,
            DateOfBirth = updated.DateOfBirth,
            Position = updated.Position,
            JerseyNumber = updated.JerseyNumber
        };
    }

    public async Task<bool> DeleteAsync(Guid id, string userRole)
    {
        if (userRole != "Admin")
        {
            _logger.LogWarning("User with role {Role} attempted to delete player {Id} without permission", userRole, id);
            throw new UnauthorizedAccessException("Only admins can delete players");
        }

        _logger.LogInformation("Deleting player {Id}", id);
        var result = await _playerRepository.DeleteAsync(id);

        if (result)
        {
            _logger.LogInformation("Player {Id} deleted successfully", id);
        }
        else
        {
            _logger.LogWarning("Player with id {Id} not found for deletion", id);
        }

        return result;
    }
}

