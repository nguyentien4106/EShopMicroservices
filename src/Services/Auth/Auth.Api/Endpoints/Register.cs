using Auth.Application.Auth.Commands.Register;
using Carter;
using Mapster;

namespace Auth.Api.Endpoints;

public class Register : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/register", async (RegisterAccountRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterAccountCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<RegisterAccountResponse>();
            
            return Results.Ok(response);
        }) .WithName("RegisterAccount")
        .Produces<RegisterAccountResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Register Account")
        .WithDescription("Register Account");
    }
}