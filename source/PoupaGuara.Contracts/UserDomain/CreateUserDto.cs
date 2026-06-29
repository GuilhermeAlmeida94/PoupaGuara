namespace PoupaGuara.Contracts.UserDomain;

// Nullable fields are intentional: validation "*-null" codes fire when fields
// are absent rather than letting model binding fail before validation runs.
public record CreateUserDto(string? Name, string? Email, DateOnly? BirthDate, string? Password);
