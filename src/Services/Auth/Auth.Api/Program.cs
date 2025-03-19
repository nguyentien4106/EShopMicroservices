using Auth.Api;
using Auth.Application;
using Auth.Infrastructure;
using Auth.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddAuthServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.UseAuthApiServices();

app.MapGet("/", () => "Auth Microservice.");

app.Run();