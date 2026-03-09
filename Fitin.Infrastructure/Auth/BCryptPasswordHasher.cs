using Fitin.Application.Authentication.Interfaces;


namespace Fitin.Infrastructure.Auth
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify (string password, string hash)
            => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}