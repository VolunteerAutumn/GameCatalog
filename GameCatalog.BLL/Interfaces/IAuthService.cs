namespace GameCatalog.BLL.Interfaces;

public interface IAuthService
{
    Task<(bool Succeeded, string[] Errors, string? Token, DateTime? ValidTo)> RegisterAsync(string email, string password);
    Task<(bool Succeeded, string? Token, DateTime? ValidTo)> LoginAsync(string email, string password);
}