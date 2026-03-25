using Fitin.Application.Common.Exceptions;
using Fitin.Application.DTOs;
using Fitin.Domain.Entities;

namespace Fitin.Application.Authentication.Interfaces;


public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task RegisterAsync(RegisterRequestDto dto)
    {
        var name = dto.Name.Trim();
        var email = dto.Email.Trim().ToLowerInvariant();
        var password = dto.Password;

        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("Name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new BadRequestException("Email is required.");

        if (string.IsNullOrWhiteSpace(password))
            throw new BadRequestException("Password is required.");

        if (!IsValidPassword(password))
        {
            throw new BadRequestException(
                "Password must be at least 8 characters and contain uppercase, lowercase, number, and special character.");
        }

        var existing = await _userRepository.GetByEmailAsync(email);

        if (existing != null)
            throw new BadRequestException("User already exists.");

        var hash = _passwordHasher.Hash(password);
        var user = new User(name, email, hash);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        // return await GenerateTokensAsync(user);
    }
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var password = dto.Password;

        if (string.IsNullOrWhiteSpace(email))
            throw new BadRequestException("Email is required.");

        if (string.IsNullOrWhiteSpace(password))
            throw new BadRequestException("Password is required.");

        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null || !_passwordHasher.Verify(password, user.PasswordHash))
            throw new BadRequestException("Invalid credentials.");
        
        if(!user.IsActive)
            throw new BadRequestException("Your account is blocked.");

        return await GenerateTokensAsync(user);
    }
    private async Task<AuthResponseDto> GenerateTokensAsync(User user)
    {
        var accessToken = _tokenGenerator.GenerateAccessToken(user);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();

        var AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15);
        var RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(1);

        var refreshEntity = new RefreshToken(
            refreshToken,
            RefreshTokenExpiresAt,
            user.Id);

        user.AddRefreshToken(refreshEntity);
        await _userRepository.AddRefreshTokenAsync(refreshEntity);

        await _userRepository.SaveChangesAsync();

        return new AuthResponseDto(
            accessToken, 
            refreshToken,
            AccessTokenExpiresAt,
            RefreshTokenExpiresAt);

    }

    private static bool IsValidPassword(string password)
    {
        return password.Length >= 8
            && password.Any(char.IsUpper)
            && password.Any(char.IsLower)
            && password.Any(char.IsDigit)
            && password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}
