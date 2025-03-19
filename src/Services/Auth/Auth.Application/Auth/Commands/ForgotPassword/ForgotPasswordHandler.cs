using Auth.Application.Exceptions;
using Auth.Domain.Models;
using BuildingBlocks.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Auth.Commands.ForgotPassword;

public class ForgotPasswordHandler(UserManager<User> userManager, JwtSettings jwtSetting) : ICommandHandler<ForgotPasswordCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email) ?? throw new UserNotFoundException(command.Email, command.Email);
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var url = $"{jwtSetting.Audience}/reset-password";
        var parameters = new Dictionary<string, string>()
        {
                { "token", token },
                { "email", command.Email },
        };

        var resetLink = new Uri(QueryHelpers.AddQueryString(url, parameters));

        return AppResponse<bool>.Success(true);
    }
}
