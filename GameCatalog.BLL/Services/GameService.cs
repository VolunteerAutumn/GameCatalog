using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Entities;
using GameCatalog.DAL.Interfaces;

namespace GameCatalog.BLL.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _repository;

    public GameService(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Game>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Game?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Game> CreateAsync(Game game)
    {
        return await _repository.CreateAsync(game);
    }

    public async Task<bool> UpdateAsync(Game game)
    {
        return await _repository.UpdateAsync(game);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}