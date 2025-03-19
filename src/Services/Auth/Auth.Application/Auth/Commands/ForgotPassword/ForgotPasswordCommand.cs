namespace Auth.Application.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : ICommand<AppResponse<bool>>;

public class ForgotPasswordValidation : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordValidation()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
    }
}

