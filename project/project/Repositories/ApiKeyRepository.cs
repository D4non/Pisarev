using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.Entities;
using Project.Repositories.Interfaces;

namespace Project.Repositories;

public class ApiKeyRepository : IApiKeyRepository
{
    private readonly ApplicationDbContext _context;

    public ApiKeyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiKey?> GetByKeyAsync(string key)
    {
        return await _context.ApiKeys
            .FirstOrDefaultAsync(k => k.Key == key);
    }
}

