using Fitin.Domain.Enums;
using Fitin.Domain.Entities.CartItems;
using Fitin.Domain.Entities.Wishlists;

namespace Fitin.Domain.Entities
{
    public class User
    {
        public Guid Id { get ; private set;}
        public string Name{get; private set;} =null!;
        public string Email {get; private set;} =null!;
        public string PasswordHash {get; private set;} =null!;
        public UserRole Role {get; private set;}
        public bool IsActive{get; private set;}
        public DateTime CreatedAt{get; private set;}

        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
        public ICollection<CartItem> CartItems {get; set;} = new List<CartItem>();

        public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
        private User(){}

        public User (string name, string email, string passwordHash ,UserRole role = UserRole.User)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
        public void AddRefreshToken(RefreshToken token)
        {
            _refreshTokens.Add(token);
        }

    }
}