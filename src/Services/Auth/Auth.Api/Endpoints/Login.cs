using Auth.Application.Auth.Commands.Login;
using Auth.Domain.ValueObjects;
using BuildingBlocks.Models;
using Carter;
using Mapster;
using Microsoft.AspNetCore.Identity.Data;

namespace Auth.Api.Endpoints;

// public record LoginResponse(AppResponse<AuthToken> Result);

public class Login : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (LoginRequest request, ISender sender) =>
            {
                var command = request.Adapt<LoginCommand>();
                var result = await sender.Send(command);
                return Results.Ok(result);
            }).WithName("LoginAccount")
        .Produces<AppResponse<AuthToken>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Login Account")
        .WithDescription("Login Account");
    }
}