using FluentValidation;
using PoupaGuara.Auth.Repositories;
using PoupaGuara.Auth.Validators;
using PoupaGuara.Endpoints;
using Scalar.AspNetCore;

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

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info.Title = "PoupaGuara API";
        document.Info.Version = "v1";
        document.Info.Description = "API do sistema PoupaGuara";
        return Task.CompletedTask;
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", () => "Hello World!");
app.MapUsuarioEndpoints();

app.Run();
