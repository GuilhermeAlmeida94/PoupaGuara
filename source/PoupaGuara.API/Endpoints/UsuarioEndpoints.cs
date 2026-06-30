using PoupaGuara.Auth.Models;
using PoupaGuara.Auth.Repositories;
using PoupaGuara.Common.Filters;

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
        .WithValidation<CreateUserDto>()
        .WithName("CriarUsuario")
        .WithSummary("Cria um novo usuário")
        .WithDescription("Cria um novo usuário no sistema com nome, e-mail, data de nascimento e senha.")
        .WithTags("Usuarios")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
