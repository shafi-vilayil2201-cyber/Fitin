namespace Fitin.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public Guid UserId { get; private set; }

        private RefreshToken() { }

        public RefreshToken(string token, DateTime expiresAt, Guid userId)
        {
            Id = Guid.NewGuid();
            Token = token;
            ExpiresAt = expiresAt;
            UserId = userId;
        }

        public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;
        public void Revoke() => IsRevoked = true;

    }
}