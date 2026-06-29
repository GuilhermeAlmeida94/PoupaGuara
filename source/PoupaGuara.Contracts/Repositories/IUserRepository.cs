using PoupaGuara.Contracts.UserDomain;

namespace PoupaGuara.Contracts.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
}
