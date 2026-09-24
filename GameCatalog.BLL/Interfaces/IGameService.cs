using GameCatalog.DAL.Entities;

namespace GameCatalog.BLL.Interfaces;

public interface IGameService
{
    Task<List<Game>> GetAllAsync();

    Task<Game?> GetByIdAsync(int id);

    Task<Game> CreateAsync(Game game);

    Task<bool> UpdateAsync(Game game);

    Task<bool> DeleteAsync(int id);
}