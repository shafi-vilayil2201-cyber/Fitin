using Fitin.Domain.Entities;

namespace Fitin.Application.Authentication.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
    Task AddAsync(User user);
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task SaveChangesAsync();
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserWithOrdersAsync(Guid userId);
}
