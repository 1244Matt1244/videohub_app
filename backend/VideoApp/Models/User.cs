#nullable enable
namespace VideoApp.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Za klasičnu autentikaciju
        public string? GoogleId { get; set; } // Za Google OAuth
        public string? GoogleEmail { get; set; }
        public bool DarkMode { get; set; } = false; // Dark mode preferencija
        public string? StripeCustomerId { get; set; } // Za Stripe integraciju
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Relacije
        public ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}