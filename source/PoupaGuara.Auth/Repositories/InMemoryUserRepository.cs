using PoupaGuara.Auth.Models;

namespace PoupaGuara.Auth.Repositories;

// In-memory stub — replace with EF Core implementation when persistence is added.
public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
}
