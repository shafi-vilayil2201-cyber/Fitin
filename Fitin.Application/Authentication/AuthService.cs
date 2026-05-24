using Fitin.Application.Common.Exceptions;
using Fitin.Application.DTOs;
using Fitin.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Fitin.Application.Authentication.Interfaces;


public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator tokenGenerator,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _configuration = configuration;
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

    public async Task<AuthResponseDto> RefreshAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new BadRequestException("Refresh token is required.");

        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

        if (user == null)
            throw new BadRequestException("Invalid or expired refresh token.");

        var storedRefreshToken = user.RefreshTokens
            .OrderByDescending(x => x.ExpiresAt)
            .FirstOrDefault(x => x.Token == refreshToken);

        if (storedRefreshToken == null || storedRefreshToken.IsRevoked || storedRefreshToken.IsExpired())
            throw new BadRequestException("Invalid or expired refresh token.");

        if (!user.IsActive)
            throw new BadRequestException("Your account is blocked.");

        storedRefreshToken.Revoke();

        return await GenerateTokensAsync(user);
    }

    public async Task LogoutAsync(string? refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return;

        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

        if (user == null)
            return;

        var storedRefreshToken = user.RefreshTokens
            .OrderByDescending(x => x.ExpiresAt)
            .FirstOrDefault(x => x.Token == refreshToken);

        if (storedRefreshToken == null || storedRefreshToken.IsRevoked)
            return;

        storedRefreshToken.Revoke();
        await _userRepository.SaveChangesAsync();
    }

    private async Task<AuthResponseDto> GenerateTokensAsync(User user)
    {
        var accessToken = _tokenGenerator.GenerateAccessToken(user);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();

        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(
            Convert.ToInt32(_configuration["Jwt:ExpiryMinutes"]));
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(1);

        var refreshEntity = new RefreshToken(
            refreshToken,
            refreshTokenExpiresAt,
            user.Id);

        user.AddRefreshToken(refreshEntity);
        await _userRepository.AddRefreshTokenAsync(refreshEntity);

        await _userRepository.SaveChangesAsync();

        return new AuthResponseDto(
            accessToken, 
            refreshToken,
            accessTokenExpiresAt,
            refreshTokenExpiresAt,
            user.Id,
            user.Name,
            user.Email,
            user.Role.ToString()
        );
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
