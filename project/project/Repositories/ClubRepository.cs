using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Project.Data;
using Project.Models.Entities;
using Project.Repositories.Interfaces;

namespace Project.Repositories;

public class ClubRepository : IClubRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _cache;
    private const string CacheKeyPrefix = "club:";

    public ClubRepository(ApplicationDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<IEnumerable<Club>> GetAllAsync()
    {
        return await _context.Clubs
            .Include(c => c.Stadium)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Club?> GetByIdAsync(int id)
    {
        var cacheKey = $"{CacheKeyPrefix}{id}";
        var cachedClub = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedClub))
        {
            try
            {
                var club = JsonSerializer.Deserialize<Club>(cachedClub);
                if (club != null)
                {
                    _context.Entry(club).State = EntityState.Detached;
                }
                return club;
            }
            catch
            {
                await _cache.RemoveAsync(cacheKey);
            }
        }

        var clubFromDb = await _context.Clubs
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (clubFromDb != null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(clubFromDb), options);
        }

        return clubFromDb;
    }

    public async Task<Club> CreateAsync(Club club)
    {
        club.CreatedAt = DateTime.UtcNow;
        _context.Clubs.Add(club);
        await _context.SaveChangesAsync();
        await _context.Entry(club).ReloadAsync();
        return club;
    }

    public async Task<Club> UpdateAsync(Club club)
    {
        var existingClub = await _context.Clubs.FindAsync(club.Id);
        if (existingClub == null)
        {
            throw new KeyNotFoundException($"Club with id {club.Id} not found");
        }

        existingClub.Name = club.Name;
        existingClub.Type = club.Type;
        existingClub.City = club.City;
        existingClub.Country = club.Country;
        existingClub.Founded = club.Founded;
        existingClub.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var cacheKey = $"{CacheKeyPrefix}{club.Id}";
        await _cache.RemoveAsync(cacheKey);

        return existingClub;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var club = await _context.Clubs.FindAsync(id);
        if (club == null)
        {
            return false;
        }

        _context.Clubs.Remove(club);
        await _context.SaveChangesAsync();

        var cacheKey = $"{CacheKeyPrefix}{id}";
        await _cache.RemoveAsync(cacheKey);

        return true;
    }

    public async Task<(IEnumerable<Club> items, int total)> GetPagedAsync(int page, int pageSize, string? search)
    {
        var query = _context.Clubs
            .Include(c => c.Stadium)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c =>
                c.Name.Contains(search) ||
                c.City.Contains(search) ||
                c.Country.Contains(search));
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }
}

