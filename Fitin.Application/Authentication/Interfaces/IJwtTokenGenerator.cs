using Fitin.Domain.Entities;

namespace Fitin.Application.Authentication.Interfaces;
public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}