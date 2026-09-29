namespace VideoApp.Dtos;

public record UserDto(
    Guid Id, string Email, string FullName,
    bool IsPremium, bool DarkMode, string? ProfilePictureUrl);
