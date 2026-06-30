using PoupaGuara.Auth.Models;

namespace PoupaGuara.Auth.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
}
