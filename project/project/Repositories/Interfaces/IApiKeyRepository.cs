using Project.Models.Entities;

namespace Project.Repositories.Interfaces;

public interface IApiKeyRepository
{
    Task<ApiKey?> GetByKeyAsync(string key);
}

