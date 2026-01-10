using Project.Models.DTO;
using Project.Models.Entities;
using Project.Repositories.Interfaces;
using Project.Services.Interfaces;

namespace Project.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IClubRepository _clubRepository;
    private readonly ILogger<MatchService> _logger;

    public MatchService(
        IMatchRepository matchRepository,
        IClubRepository clubRepository,
        ILogger<MatchService> logger)
    {
        _matchRepository = matchRepository;
        _clubRepository = clubRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<MatchDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all matches");
        var matches = await _matchRepository.GetAllAsync();
        return matches.Select(m => new MatchDto
        {
            Id = m.Id,
            HomeClubId = m.HomeClubId,
            HomeClubName = m.HomeClub.Name,
            AwayClubId = m.AwayClubId,
            AwayClubName = m.AwayClub.Name,
            StadiumId = m.StadiumId,
            StadiumName = m.Stadium?.Name,
            MatchDate = m.MatchDate,
            HomeScore = m.HomeScore,
            AwayScore = m.AwayScore,
            Status = m.Status.ToString()
        });
    }

    public async Task<MatchDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting match by id {Id}", id);
        var match = await _matchRepository.GetByIdAsync(id);

        if (match == null)
        {
            _logger.LogWarning("Match with id {Id} not found", id);
            return null;
        }

        return new MatchDto
        {
            Id = match.Id,
            HomeClubId = match.HomeClubId,
            HomeClubName = match.HomeClub.Name,
            AwayClubId = match.AwayClubId,
            AwayClubName = match.AwayClub.Name,
            StadiumId = match.StadiumId,
            StadiumName = match.Stadium?.Name,
            MatchDate = match.MatchDate,
            HomeScore = match.HomeScore,
            AwayScore = match.AwayScore,
            Status = match.Status.ToString()
        };
    }

    public async Task<MatchDto> CreateAsync(CreateMatchDto dto, string userRole)
    {
        if (userRole != "Admin" && userRole != "Manager")
        {
            _logger.LogWarning("User with role {Role} attempted to create match without permission", userRole);
            throw new UnauthorizedAccessException("You do not have permission to create matches");
        }

        _logger.LogInformation("Creating new match between clubs {HomeClubId} and {AwayClubId}", dto.HomeClubId, dto.AwayClubId);

        var homeClub = await _clubRepository.GetByIdAsync(dto.HomeClubId);
        if (homeClub == null)
        {
            throw new KeyNotFoundException($"Home club with id {dto.HomeClubId} not found");
        }

        var awayClub = await _clubRepository.GetByIdAsync(dto.AwayClubId);
        if (awayClub == null)
        {
            throw new KeyNotFoundException($"Away club with id {dto.AwayClubId} not found");
        }

        var match = new Match
        {
            HomeClubId = dto.HomeClubId,
            AwayClubId = dto.AwayClubId,
            StadiumId = dto.StadiumId,
            MatchDate = dto.MatchDate,
            Status = MatchStatus.Scheduled
        };

        var created = await _matchRepository.CreateAsync(match);
        _logger.LogInformation("Match {Id} created successfully", created.Id);

        return new MatchDto
        {
            Id = created.Id,
            HomeClubId = created.HomeClubId,
            HomeClubName = homeClub.Name,
            AwayClubId = created.AwayClubId,
            AwayClubName = awayClub.Name,
            StadiumId = created.StadiumId,
            MatchDate = created.MatchDate,
            Status = created.Status.ToString()
        };
    }

    public async Task<MatchDto> UpdateAsync(Guid id, UpdateMatchDto dto, string userRole)
    {
        if (userRole != "Admin" && userRole != "Manager")
        {
            _logger.LogWarning("User with role {Role} attempted to update match {Id} without permission", userRole, id);
            throw new UnauthorizedAccessException("You do not have permission to update matches");
        }

        _logger.LogInformation("Updating match {Id}", id);

        var match = await _matchRepository.GetByIdAsync(id);
        if (match == null)
        {
            _logger.LogWarning("Match with id {Id} not found for update", id);
            throw new KeyNotFoundException($"Match with id {id} not found");
        }

        match.StadiumId = dto.StadiumId;
        match.MatchDate = dto.MatchDate;
        match.HomeScore = dto.HomeScore;
        match.AwayScore = dto.AwayScore;
        match.Status = (MatchStatus)dto.Status;

        var updated = await _matchRepository.UpdateAsync(match);
        _logger.LogInformation("Match {Id} updated successfully", updated.Id);

        return new MatchDto
        {
            Id = updated.Id,
            HomeClubId = updated.HomeClubId,
            HomeClubName = updated.HomeClub.Name,
            AwayClubId = updated.AwayClubId,
            AwayClubName = updated.AwayClub.Name,
            StadiumId = updated.StadiumId,
            StadiumName = updated.Stadium?.Name,
            MatchDate = updated.MatchDate,
            HomeScore = updated.HomeScore,
            AwayScore = updated.AwayScore,
            Status = updated.Status.ToString()
        };
    }

    public async Task<bool> DeleteAsync(Guid id, string userRole)
    {
        if (userRole != "Admin")
        {
            _logger.LogWarning("User with role {Role} attempted to delete match {Id} without permission", userRole, id);
            throw new UnauthorizedAccessException("Only admins can delete matches");
        }

        _logger.LogInformation("Deleting match {Id}", id);
        var result = await _matchRepository.DeleteAsync(id);

        if (result)
        {
            _logger.LogInformation("Match {Id} deleted successfully", id);
        }
        else
        {
            _logger.LogWarning("Match with id {Id} not found for deletion", id);
        }

        return result;
    }
}

