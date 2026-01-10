using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.Entities;
using Project.Repositories.Interfaces;

namespace Project.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        return await _context.Players
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();
    }

    public async Task<Player?> GetByIdAsync(Guid id)
    {
        return await _context.Players
            .Include(p => p.ClubPlayers)
                .ThenInclude(cp => cp.Club)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Player> CreateAsync(Player player)
    {
        player.Id = Guid.NewGuid();
        player.CreatedAt = DateTime.UtcNow;
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task<Player> UpdateAsync(Player player)
    {
        player.UpdatedAt = DateTime.UtcNow;
        _context.Players.Update(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null)
        {
            return false;
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return true;
    }
}

