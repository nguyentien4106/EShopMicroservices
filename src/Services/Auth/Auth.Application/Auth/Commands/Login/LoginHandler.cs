using System.Security.Claims;
using System.Text;
using Auth.Application.Exceptions;
using Auth.Application.Helpers;
using Auth.Domain.Models;
using Auth.Domain.ValueObjects;
using BuildingBlocks.Auth.Models;
using BuildingBlocks.CQRS;
using BuildingBlocks.Models;

namespace Auth.Application.Auth.Commands.Login;

public class LoginHandler(SignInManager<User> signInManager, UserManager<User> userManager, JwtSettings jwtSettings) : ICommandHandler<LoginCommand, AppResponse<AuthToken>>
{
    public async Task<AppResponse<AuthToken>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = userManager.FindByEmailAsync(command.Email).Result;

        if (user is null)
        {
            throw new UserNotFoundException("User", command.Email);
        }
        
        var result = await signInManager.PasswordSignInAsync(user, command.Password, true, true);
        if (result.Succeeded)
        {
            var token = await GenerateUserToken(user);
            return AppResponse<AuthToken>.Success(token);
        }

        if (result.IsLockedOut) 
        {
            return AppResponse<AuthToken>.Error("Your account has been locked out");
        }

        if (result.IsNotAllowed)
        {
            return AppResponse<AuthToken>.Error("Your account hasn't been allowed");
        }

        if (result.RequiresTwoFactor)
        {
            return AppResponse<AuthToken>.Error("Your account didn't turn the two factor on! Please turn 2FA on first.");
        }

        return AppResponse<AuthToken>.Error("Password was incorrect! Please try again.");
    }

    private async Task<AuthToken> GenerateUserToken(User user)
    {
       
        var accessToken = await TokenUtils.GenerateAccessToken(userManager, jwtSettings, user);
        var refreshToken =  await TokenUtils.GenerateRefreshToken(userManager, jwtSettings, user);
        
        return new AuthToken
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }
}