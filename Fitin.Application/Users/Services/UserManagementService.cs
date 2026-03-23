

using AutoMapper;
using Fitin.Application.Authentication.Interfaces;
using Fitin.Application.Common.Exceptions;
using Fitin.Application.Common.Interfaces;
using Fitin.Application.Users.DTOs;
using Fitin.Application.Users.Interfaces;
using Fitin.Domain.Enums;
  
namespace Fitin.Application.Users.Services;

public class UserManagementService : IUserManagementService
{

    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserManagementService (
            IUserRepository userRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserListItemDto>> GetUsersAsync()
    {
        var result = await _userRepository.GetUsersAsync();
        return _mapper.Map<IEnumerable<UserListItemDto>>(result);
    }
    public async Task<UserDetailsDto?> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetUserWithOrdersAsync(userId);

        if(user == null)
            return null;

        return _mapper.Map<UserDetailsDto>(user);
    }
    public async Task UpdateUserRoleAsync(Guid userId, UserRole role)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if(user == null)
            throw new NotFoundException("User not found");
        
        try
        {
            user.UpdateRole(role);
        }
        catch(InvalidOperationException ex)
        {
            throw new BadRequestException(ex.Message);
        }
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task BlockUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if(user == null)
            throw new NotFoundException("User not found");

        try
        {
            user.Block();
        }
        catch (InvalidOperationException ex)
        {
            
            throw new BadRequestException(ex.Message);
        }
        await _unitOfWork.SaveChangesAsync();

    }
    public async Task UnblockUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new NotFoundException("User not found");

        try
        {
            user.Unblock();
        }
        catch (InvalidOperationException ex)
        {
            
            throw new BadRequestException(ex.Message);
        }
        await _unitOfWork.SaveChangesAsync();
    }

}

    
