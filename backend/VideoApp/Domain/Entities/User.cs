namespace VideoApp.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? GoogleId { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool IsPremium { get; set; }
    public DateTime? PremiumExpiresAt { get; set; }
    public bool DarkMode { get; set; }
    public DateTime CreatedAt { get; set; }
}
