using Auth.Application.Data;
using Auth.Application.Enums;
using Auth.Domain.Models;
using BuildingBlocks.CQRS;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Auth.Commands.Register;

public class RegisterAccountHandler(IAuthDbContext dbContext, UserManager<User> userManager) : ICommandHandler<RegisterAccountCommand, RegisterAccountResult>
{
    public async Task<RegisterAccountResult> Handle(RegisterAccountCommand command, CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            Email = command.Email,
            UserName = command.UserName,
            PhoneNumber = command.PhoneNumber,
            FirstName = command.FirstName,
            LastName = command.LastName,
            
        };
        var result = await userManager.CreateAsync(newUser, command.Password);
        if (result.Succeeded)
        {
            newUser.Status = (int)AccountStatus.Active;
        }
        
        return new RegisterAccountResult(result);
    }
}