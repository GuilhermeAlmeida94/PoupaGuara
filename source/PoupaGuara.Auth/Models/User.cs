namespace PoupaGuara.Auth.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public string PasswordHash { get; set; } = string.Empty; // never store plain-text password
}
