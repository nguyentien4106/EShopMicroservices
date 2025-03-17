using Auth.Application.Data;
using Carter;

namespace Auth.Api.Endpoints;

public class GetProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/profile/{userName}", async (IAuthDbContext dbContext, string userName) =>
        {
            var user = dbContext.ApplicationUsers.FirstOrDefault(item => item.UserName == userName);
            return user?.UserName ?? "Profile";
        })
        .WithName("GetProfile")
        .Produces(StatusCodes.Status202Accepted)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();
        
    }
}