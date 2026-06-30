using PoupaGuara.Auth.Models;
using PoupaGuara.Auth.Repositories;
using PoupaGuara.Common.Filters;

namespace PoupaGuara.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/users", async (CreateUserDto dto, IUserRepository repo) =>
        {
            var user = new User
            {
                Name = dto.Name!,
                Email = dto.Email!,
                BirthDate = dto.BirthDate!.Value,
                PasswordHash = dto.Password! // TODO: apply BCrypt/Argon2 hash when auth is wired
            };

            await repo.AddAsync(user);
            return Results.Created($"/users/{user.Id}", new { user.Id });
        })
        .WithValidation<CreateUserDto>()
        .WithName("CreateUser")
        .WithSummary("Creates a new user")
        .WithDescription("Creates a new user in the system with name, email, birth date and password.")
        .WithTags("Users")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
