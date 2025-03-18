using Basket.API.Data;
using BuildingBlocks.Auth.AuthConfiguration;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;
var databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;

builder.Services.AddCarter();
builder.Services.AddJwtServices();
builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
    {
        opts.Connection(databaseConnectionString);
        opts.Schema.For<ShoppingCart>().Identity(p => p.UserName);
    }).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(opts =>
    {
        opts.Configuration = redisConnectionString;
    });

builder.Services
    .AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opts =>
    {
        opts.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        return handler;
    });

builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services
    .AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString);

var app = builder.Build();

app.UseJwtServices();

app.MapCarter();

app.MapGet("/", () => "Basket API");

app.UseExceptionHandler(opts => {  });

app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
