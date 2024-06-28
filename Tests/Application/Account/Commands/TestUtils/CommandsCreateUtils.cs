using Microsoft.AspNetCore.WebUtilities;
using PizzaApi.Application.Users.Commands.ChangeName;
using PizzaApi.Application.Users.Commands.ChangePassword;
using PizzaApi.Application.Users.Commands.ConfirmAccount;
using PizzaApi.Application.Users.Commands.ConfirmNewEmail;
using PizzaApi.Application.Users.Commands.Login;
using PizzaApi.Application.Users.Commands.RefreshToken;
using PizzaApi.Application.Users.Commands.Register;
using PizzaApi.Application.Users.Commands.ResetPassword;
using PizzaApi.Tests.Application.TestUtils.Constants;
using System.Text;

namespace PizzaApi.Tests.Application.Account.Commands.TestUtils
{
    internal static class CommandsCreateUtils
    {
        public static ConfirmAccountCommand CreateConfirmAccountCommand(
            Guid? userId = null,
            string code = null)
        {
            return new ConfirmAccountCommand(
                userId ?? Guid.NewGuid(),
                code ?? WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("validCode")));
        }

        public static ConfirmNewEmailCommand CreateConfirmNewEmailCommand(
            Guid? userId = null,
            string code = null,
            string email = null)
        {
            return new ConfirmNewEmailCommand(
                userId ?? Guid.NewGuid(),
                code ?? WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("validCode")),
                email ?? Constants.User.Email);
        }

        public static LoginCommand CreateLoginCommand(
            string email = null,
            string password = null,
            bool? useCookies = null,
            bool? useSessionCookies = null)
        {
            return new LoginCommand(
                email ?? Constants.User.Email,
                password ?? Constants.User.Password,
                useCookies ?? true,
                useSessionCookies ?? false);
        }

        public static RegisterCommand CreateRegisterCommand(
            string name = null,
            string email = null,
            string password = null)
        {
            return new RegisterCommand(
                name ?? Constants.User.Name,
                email ?? Constants.User.Email,
                password ?? Constants.User.Password);
        }

        public static ResetPasswordCommand CreateResetPasswordCommand(
           string email = null,
           string resetCode = null,
           string newPassword = null)
        {
            return new ResetPasswordCommand(
                email ?? Constants.User.Email,
                resetCode ?? WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("validResetCode")),
                newPassword ?? Constants.User.Password);
        }

        public static ChangePasswordCommand CreateChangePasswordCommand(
            Guid? userId = null,
            string oldPassword = null,
            string newPassword = null)
        {
            return new ChangePasswordCommand(
                userId ?? Guid.NewGuid(),
                oldPassword ?? Constants.User.Password,
                newPassword ?? "NewStrongPassword123!");
        }

        public static RefreshTokenCommand CreateRefreshTokenCommand(string refreshToken = null)
        {
            return new RefreshTokenCommand(
                refreshToken ?? "validRefreshToken");
        }

        public static ChangeNameCommand CreateChangeNameCommand(Guid? userId = null, string newName = null)
        {
            return new ChangeNameCommand(
                userId ?? Guid.NewGuid(),
                newName ?? Constants.User.Name);
        }

    }
}
