using GameCatalog.DAL.Entities;

namespace GameCatalog.BLL.Interfaces;

public interface ITokenService
{
    Task<(string Token, DateTime ValidTo)> CreateTokenAsync(ApplicationUser user);
}