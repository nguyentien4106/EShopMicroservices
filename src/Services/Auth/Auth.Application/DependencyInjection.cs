using System.Reflection;
using Auth.Application.Services;
using Auth.Domain.Models;
using BuildingBlocks.Behaviors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        
        services.AddSingleton<IEmailSender<User>, EmailService>();
        
        var sendGridSettings = configuration.GetSection("SendGridSettings").Get<SendGridSettings>() ?? throw new InvalidOperationException("SendGridSettings not found.");
        services.AddSingleton(sendGridSettings);

        services.AddFeatureManagement();
        
        return services;
    }
}