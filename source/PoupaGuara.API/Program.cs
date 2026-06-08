var builder = WebApplication.CreateBuilder(args);builder.Services.AddCors(options =>

{

    options.AddPolicy(name: "MyAllowSpecificOrigins",

                      policy =>

                      {

                          policy.WithOrigins("http://example.com",

                                              "http://www.contoso.com");

                      });

});


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
