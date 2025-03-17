using Auth.Api;
using Auth.Application;
using Auth.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddAuthServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();
// builder.
var app = builder.Build();

app.UseAuthApiServices();

app.MapGet("/", () => "Auth Microservice.");

app.Run();