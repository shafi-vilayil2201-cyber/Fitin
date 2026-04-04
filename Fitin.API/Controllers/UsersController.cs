
using Fitin.Application.Users.DTOs;
using Fitin.Application.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController : BaseApiController
{
    private readonly IUserManagementService _service;

    public UsersController(IUserManagementService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _service.GetUsersAsync();
        return Success(users,"Users fetched successfully");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _service.GetUserByIdAsync(id);

        if(user == null)
            return Failure("User not Found",statusCode:404);

        return Success(user,"User fetched successfully");
    }

    [HttpPatch("{id:guid}/role")]
    public async Task<IActionResult> UpdateRole(Guid id,UpdateUserRoleDto dto )
    {
        await _service.UpdateUserRoleAsync(id,dto.Role);
        return Success<Object?>(null,"User role updated successfully");
    }

    [HttpPatch("{id:guid}/block")]
    public async Task<IActionResult> BlockUser(Guid id)
    {
        var currentUserIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(currentUserIdStr, out var currentUserId))
        {
            if (id == currentUserId)
                return Failure("You cannot block your own account");
        }

        var targetUser = await _service.GetUserByIdAsync(id);
        if (targetUser == null)
            return Failure("User not found", statusCode: 404);

        if (targetUser.Role == Fitin.Domain.Enums.UserRole.Admin)
            return Failure("Administrative accounts cannot be blocked");

        await _service.BlockUserAsync(id);
        return Success<object?>(null, "User blocked successfully");
    }

    [HttpPatch("{id:guid}/unblock")]
    public async Task<IActionResult> UnblockUser(Guid id)
    {
        await _service.UnblockUserAsync(id);
        return Success<Object?> (null,"User unblocked successfully");
    }
                                                                                                              

}