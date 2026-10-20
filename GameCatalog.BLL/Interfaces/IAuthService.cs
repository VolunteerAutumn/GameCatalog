namespace GameCatalog.BLL.Interfaces;

public interface IAuthService
{
    Task<(bool Succeeded, string[] Errors)> RegisterAsync(string email, string password);
    Task<(bool Succeeded, string? Token, DateTime? ValidTo)> LoginAsync(string email, string password);

    Task<bool> AssignRoleAsync(string email, string roleName);
    Task<bool> ConfirmEmailAsync(string userId, string token);

    Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword);
    Task<bool> ForgotPasswordAsync(string email);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    Task<(bool Succeeded, string? Token)> RequestEmailChangeAsync(string currentEmail, string newEmail);
    Task<bool> ConfirmEmailChangeAsync(string userId, string newEmail, string token);
}