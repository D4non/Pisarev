using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Project.Data;
using Project.Models.Entities;
using Project.Repositories.Interfaces;

namespace Project.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;

    public MatchRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public async Task<IEnumerable<Match>> GetAllAsync()
    {
        return await _context.Matches
            .Include(m => m.HomeClub)
            .Include(m => m.AwayClub)
            .Include(m => m.Stadium)
            .OrderByDescending(m => m.MatchDate)
            .ToListAsync();
    }

    public async Task<Match?> GetByIdAsync(Guid id)
    {
        return await _context.Matches
            .Include(m => m.HomeClub)
            .Include(m => m.AwayClub)
            .Include(m => m.Stadium)
            .Include(m => m.PlayerMatches)
                .ThenInclude(pm => pm.Player)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Match> CreateAsync(Match match)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            match.Id = Guid.NewGuid();
            match.CreatedAt = DateTime.UtcNow;

            var sql = @"
                INSERT INTO matches (id, home_club_id, away_club_id, stadium_id, match_date, home_score, away_score, status, created_at)
                VALUES (@Id, @HomeClubId, @AwayClubId, @StadiumId, @MatchDate, @HomeScore, @AwayScore, @Status, @CreatedAt)";

            var parameters = new DynamicParameters();
            parameters.Add("Id", match.Id);
            parameters.Add("HomeClubId", match.HomeClubId);
            parameters.Add("AwayClubId", match.AwayClubId);
            parameters.Add("StadiumId", match.StadiumId);
            parameters.Add("MatchDate", match.MatchDate);
            parameters.Add("HomeScore", match.HomeScore);
            parameters.Add("AwayScore", match.AwayScore);
            parameters.Add("Status", (int)match.Status);
            parameters.Add("CreatedAt", match.CreatedAt);

            await connection.ExecuteAsync(sql, parameters, transaction);

            await transaction.CommitAsync();

            return match;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Match> UpdateAsync(Match match)
    {
        match.UpdatedAt = DateTime.UtcNow;
        _context.Matches.Update(match);
        await _context.SaveChangesAsync();
        return match;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var match = await _context.Matches.FindAsync(id);
        if (match == null)
        {
            return false;
        }

        _context.Matches.Remove(match);
        await _context.SaveChangesAsync();
        return true;
    }
}

