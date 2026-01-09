using Project.Models.Entities;

namespace Project.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
}

