using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Abstractions.Token;
using blogapp_server.Application.Dtos;
using blogapp_server.Application.Dtos.Auth;
using blogapp_server.Application.Exceptions;
using blogapp_server.Application.Helpers;
using blogapp_server.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenHandler tokenHandler,  IUserService userService, IMailService mailService, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _userService = userService;
            _mailService = mailService;
            _logger = logger;
        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password)
        {
            var user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
                if (user == null)
                {
                    _logger.LogWarning("Login failed. Reason: {Reason}", "UserNotFound");
                    throw new UnauthorizedAccesException("Kullanıcı adı veya şifre hatalı!");
                }
            }

            if (user.Status == Domain.Enums.UserStatus.Suspended && user.SuspendedUntil.HasValue && user.SuspendedUntil.Value <= DateTime.UtcNow)
            {
                user.Status = Domain.Enums.UserStatus.Active;
                user.SuspendedUntil = null;
                user.LockoutEnd = null;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    throw new UnauthorizedAccesException("Kullanıcı hesabı güncellenemedi.");
                }
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                if (user.Status == Domain.Enums.UserStatus.Banned)
                {
                    _logger.LogWarning("Login blocked. Reason: {Reason}, UserId: {UserId}", "UserBanned", user.Id);
                    throw new UnauthorizedAccesException("Bu hesap platformdan yasaklanmıştır.");
                }

                if (user.Status == Domain.Enums.UserStatus.Suspended)
                {
                    _logger.LogWarning("Login blocked. Reason: {Reason}, UserId: {UserId}", "UserSuspended", user.Id);
                    throw new UnauthorizedAccesException("Bu hesap geçici olarak askıya alınmıştır.");
                }

                var roles = await _userManager.GetRolesAsync(user);
                Token token = _tokenHandler.CreateAccessToken(user, roles);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration);

                _logger.LogInformation(
                    "Login succeeded. UserId: {UserId}, UserName: {UserName}, RolesCount: {RolesCount}",
                    user.Id,
                    user.UserName,
                    roles.Count);

                return token;
            }
            else
            {
                if (user.Status == Domain.Enums.UserStatus.Banned)
                {
                    _logger.LogWarning("Login blocked. Reason: {Reason}, UserId: {UserId}", "UserBanned", user.Id);
                    throw new UnauthorizedAccesException("Bu hesap platformdan yasaklanmıştır.");
                }

                if (user.Status == Domain.Enums.UserStatus.Suspended)
                {
                    _logger.LogWarning("Login blocked. Reason: {Reason}, UserId: {UserId}", "UserSuspended", user.Id);
                    throw new UnauthorizedAccesException("Bu hesap geçici olarak askıya alınmıştır.");
                }

                _logger.LogWarning(
                    "Login failed. Reason: {Reason}, UserId: {UserId}, UserName: {UserName}",
                    "InvalidPassword",
                    user.Id,
                    user.UserName);

                throw new UnauthorizedAccesException("Kullanıcı adı veya şifre hatalı!");
            }
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            User? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                if (user.Status == Domain.Enums.UserStatus.Banned)
                {
                    _logger.LogWarning("Refresh token login blocked. Reason: {Reason}, UserId: {UserId}", "UserBanned", user.Id);
                    throw new UnauthorizedAccesException("Bu hesap platformdan yasaklanmıştır.");
                }

                if (user.Status == Domain.Enums.UserStatus.Suspended)
                {
                    if (!user.SuspendedUntil.HasValue || user.SuspendedUntil.Value > DateTime.UtcNow)
                    {
                        _logger.LogWarning("Refresh token login blocked. Reason: {Reason}, UserId: {UserId}", "UserSuspended", user.Id);
                        throw new UnauthorizedAccesException("Bu hesap geçici olarak askıya alınmıştır.");
                    }

                    user.Status = Domain.Enums.UserStatus.Active;
                    user.SuspendedUntil = null;
                    user.LockoutEnd = null;
                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        throw new UnauthorizedAccesException("Kullanıcı hesabı güncellenemedi.");
                    }
                }

                var roles = await _userManager.GetRolesAsync(user);
                Token token = _tokenHandler.CreateAccessToken(user, roles);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration);

                _logger.LogInformation(
                    "Refresh token login succeeded. UserId: {UserId}, UserName: {UserName}, RolesCount: {RolesCount}",
                    user.Id,
                    user.UserName,
                    roles.Count);

                return token;
            }
            else
            {
                _logger.LogWarning("Refresh token login failed. Reason: {Reason}", "InvalidOrExpiredRefreshToken");
                throw new NotFoundException("Kullanıcı bulunumadı!");
            }
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordResetAsync(ForgotPasswordRequest request)
        {
            ForgotPasswordResponse response = new()
            {
                Succeeded = true,
                Message = "Mail adresi doğru ise Şifre Sıfırlama bağlantısı gönderildi"
            };

            if (string.IsNullOrWhiteSpace(request.EmailOrUsername))
            {
                return response;
            }

            User user = await _userManager.FindByEmailAsync(request.EmailOrUsername);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.EmailOrUsername);
                if (user == null)
                {
                    return response;
                }
            }

            // Reset token üretiliyor
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            resetToken = resetToken.UrlEncode();

            await _mailService.SendForgotPasswordMailAsync(user.Email, user.FullName, user.Id, resetToken);
            return response;
        }


        public async Task<MailVerifyResponse> MailVerifyAsync(MailVerifyRequest request)
        {
            MailVerifyResponse response = new()
            {
                Succeeded = true,
                Message = "Mail adresi doğru ise doğrulama bağlantısı gönderildi"
            };

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return response;
            }

            User user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return response;
            }

            if (user.EmailConfirmed == true)
            {
                return new MailVerifyResponse
                {
                    Succeeded = true,
                    Message = "E-posta adresi zaten doğrulanmış."
                };
            }

            string emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            emailConfirmToken = emailConfirmToken.UrlEncode();

            await _mailService.SendVerifyMailAsync(user.Email, user.FullName, user.Id, emailConfirmToken);
            return response;
        }

        public async Task<ChangeEmailResponse> ChangeEmailAsync(ChangeEmailRequest request)
        {
            ChangeEmailResponse response = new()
            {
                Succeeded = true,
                Message = "Yeni e-posta adresiniz doğrulama için kontrol edilmelidir. Uygunsa doğrulama bağlantısı gönderildi."
            };

            if (string.IsNullOrWhiteSpace(request.NewEmail))
            {
                return response;
            }

            User user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return response;
            }

            string newEmail = request.NewEmail.Trim().ToLower();
            User user2 = await _userManager.FindByEmailAsync(newEmail);
            if (user2 != null)
            {
                return response;
            }

            string emailChangeToken = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            emailChangeToken = emailChangeToken.UrlEncode();

            await _mailService.SendChangeEmailMailAsync(newEmail, user.FullName, user.Id, emailChangeToken);
            return response;
        }

        public async Task<ChangePhoneNumberResponse> ChangePhoneNumberAsync(ChangePhoneNumberRequest request)
        {
            ChangePhoneNumberResponse response = new()
            {
                Succeeded = true,
                Message = "Telefon numarası uygunsa doğrulama kodu e-posta adresinize gönderildi."
            };

            if (string.IsNullOrWhiteSpace(request.NewPhoneNumber))
            {
                return response;
            }

            User user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return response;
            }
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return response;
            }

            string newPhoneNumber = request.NewPhoneNumber.Trim();
            newPhoneNumber = PhoneNumberHelper.NormalizeTurkeyPhoneNumber(request.NewPhoneNumber);

            bool phoneNumberExists = await _userManager.Users.AnyAsync(u => u.PhoneNumber == newPhoneNumber && u.Id != user.Id);
            if (phoneNumberExists == true)
            {
                return response;
            }

            string token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, newPhoneNumber);

            await _mailService.SendChangePhoneNumberMailAsync(user.Email, user.FullName, user.Id, newPhoneNumber, token);
            return response;
        }
    }
}
