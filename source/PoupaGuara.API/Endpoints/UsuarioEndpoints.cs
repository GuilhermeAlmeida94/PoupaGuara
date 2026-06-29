using PoupaGuara.Auth.Filters;
using PoupaGuara.Contracts.Repositories;
using PoupaGuara.Contracts.UserDomain;

namespace PoupaGuara.Endpoints;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this WebApplication app)
    {
        app.MapPost("/usuarios", async (CreateUserDto dto, IUserRepository repo) =>
        {
            var user = new User
            {
                Name = dto.Name!,
                Email = dto.Email!,
                BirthDate = dto.BirthDate!.Value,
                PasswordHash = dto.Password! // TODO: apply BCrypt/Argon2 hash when auth is wired
            };

            await repo.AddAsync(user);
            return Results.Created($"/usuarios/{user.Id}", new { user.Id });
        })
        .WithValidation<CreateUserDto>();
    }
}
