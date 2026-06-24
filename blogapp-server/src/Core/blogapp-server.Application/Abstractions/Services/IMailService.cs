using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Abstractions.Services
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string subject, string content);
        //Birden fazla kişiye mail gönderilecekse
        Task SendMailAsync(string[] toes, string subject, string content);
        Task SendForgotPasswordMailAsync(string to, string userFullName, string verificationCode);
        Task SendVerifyMailAsync(string to, string fullName, string verificationCode);
        Task SendChangeEmailOldMailAsync(string to, string fullName, string newEmail, string verificationCode);
        Task SendChangeEmailMailAsync(string to, string fullName, string verificationCode);
    }
}
