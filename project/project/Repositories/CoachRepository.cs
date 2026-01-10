using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.Entities;
using Project.Repositories.Interfaces;

namespace Project.Repositories;

public class CoachRepository : ICoachRepository
{
    private readonly ApplicationDbContext _context;

    public CoachRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Coach>> GetAllAsync()
    {
        return await _context.Coaches
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToListAsync();
    }

    public async Task<Coach?> GetByIdAsync(Guid id)
    {
        return await _context.Coaches
            .Include(c => c.ClubCoaches)
                .ThenInclude(cc => cc.Club)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Coach> CreateAsync(Coach coach)
    {
        coach.Id = Guid.NewGuid();
        coach.CreatedAt = DateTime.UtcNow;
        _context.Coaches.Add(coach);
        await _context.SaveChangesAsync();
        return coach;
    }

    public async Task<Coach> UpdateAsync(Coach coach)
    {
        coach.UpdatedAt = DateTime.UtcNow;
        _context.Coaches.Update(coach);
        await _context.SaveChangesAsync();
        return coach;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var coach = await _context.Coaches.FindAsync(id);
        if (coach == null)
        {
            return false;
        }

        _context.Coaches.Remove(coach);
        await _context.SaveChangesAsync();
        return true;
    }
}

