using blogapp_server.Application.Abstractions.Services;

namespace blogapp_server.IntegrationTests.Fixtures;

public sealed class TestMailService : IMailService
{
    public Task SendMailAsync(string to, string subject, string content) => Task.CompletedTask;
    public Task SendMailAsync(string[] toes, string subject, string content) => Task.CompletedTask;
    public Task SendForgotPasswordMailAsync(string to, string userFullName, string verificationCode) => Task.CompletedTask;
    public Task SendVerifyMailAsync(string to, string fullName, string verificationCode) => Task.CompletedTask;
    public Task SendChangeEmailOldMailAsync(string to, string fullName, string newEmail, string verificationCode) => Task.CompletedTask;
    public Task SendChangeEmailMailAsync(string to, string fullName, string verificationCode) => Task.CompletedTask;
}
