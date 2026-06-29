using FluentValidation;
using PoupaGuara.Auth.Repositories;
using PoupaGuara.Auth.Validators;
using PoupaGuara.Contracts.Repositories;
using PoupaGuara.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://example.com",
                               "http://www.contoso.com");
        });
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapUsuarioEndpoints();

app.Run();
