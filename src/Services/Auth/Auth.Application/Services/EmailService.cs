using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using Auth.Domain.Models;

namespace Auth.Application.Services;

public class EmailService(IOptions<SendGridSettings> settings) : IEmailSender<User>
{
    private readonly SendGridSettings _settings = settings.Value;

    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        var subject = "Confirm your email";
        var body = $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.";
        await SendEmailAsync(email, subject, body, true);
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        var subject = "Reset your password";
        var body = $"Click <a href='{resetLink}'>here</a> to reset your password.";
        await SendEmailAsync(email, subject, body, true);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml)
    {
        var client = new SendGridClient(_settings.ApiKey);
        var from = new EmailAddress(_settings.FromEmail, _settings.SenderName);
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, isHtml ? null : body, isHtml ? body : null);

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Send email to {toEmail} Failed. Status : {response.StatusCode.ToString()}");
        }
    }
}
