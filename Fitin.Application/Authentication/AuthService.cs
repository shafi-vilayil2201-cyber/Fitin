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

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        var existing = await _userRepository.GetByEmailAsync(dto.Email);

        if (existing != null)
            throw new Exception("User Already exists");

        var hash = _passwordHasher.Hash(dto.Password);
        var user = new User(dto.Name, dto.Email, hash);

        await _userRepository.AddAsync(user);

        return await GenerateTokensAsync(user);
    }
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Invalid credential");

        return await GenerateTokensAsync(user);
    }
    private async Task<AuthResponseDto> GenerateTokensAsync(User user)
    {
        var accessToken = _tokenGenerator.GenerateAccessToken(user);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();

        var refreshEntity = new RefreshToken(
            refreshToken,
            DateTime.UtcNow.AddDays(1),
            user.Id);

        user.AddRefreshToken(refreshEntity);
        await _userRepository.AddRefreshTokenAsync(refreshEntity);

        await _userRepository.SaveChangesAsync();

        return new AuthResponseDto(accessToken, refreshToken);

        

    }
}
