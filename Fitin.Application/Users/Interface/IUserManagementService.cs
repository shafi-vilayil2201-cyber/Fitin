

using Fitin.Application.Users.DTOs;
using Fitin.Domain.Enums;

namespace Fitin.Application.Users.Interfaces;

public interface IUserManagementService
{
    Task<IEnumerable<UserListItemDto>> GetUsersAsync();
    Task<UserDetailsDto?> GetUserByIdAsync(Guid userId);
    Task UpdateUserRoleAsync(Guid userId,UserRole role);
    Task BlockUserAsync(Guid userId);
    Task UnblockUserAsync(Guid userId);
}

