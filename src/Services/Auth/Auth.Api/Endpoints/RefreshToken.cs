using Auth.Application.Auth.Commands.RefreshToken;
using BuildingBlocks.Models;
using Carter;
using Mapster;

namespace Auth.Api.Endpoints;

public class RefreshToken : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/refresh-token", async (AuthToken request, ISender sender) =>
        {
            var command = request.Adapt<RefreshTokenCommand>();
            var result = await sender.Send(command);

            return Results.Ok(result);
        }).WithName("refresh-token")
        .Produces<AppResponse<AuthToken>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithDescription("refresh a token")
        .WithSummary("refresh a token");
    }
    
}