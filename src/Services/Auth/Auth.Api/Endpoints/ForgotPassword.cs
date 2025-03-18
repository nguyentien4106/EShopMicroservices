using Carter;

namespace Auth.Api.Endpoints;

public class ForgotPassword : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/forgot-password", async (ISender sender) =>
        {
            
        });
    }
}