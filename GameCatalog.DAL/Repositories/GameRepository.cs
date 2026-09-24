using GameCatalog.DAL.Data;
using GameCatalog.DAL.Entities;
using GameCatalog.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameCatalog.DAL.Repositories;

public class GameRepository : IGameRepository
{
    private readonly GameCatalogDbContext _context;

    public GameRepository(GameCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<List<Game>> GetAllAsync()
    {
        return await _context.Games
            .Include(g => g.Developer)
            .ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(int id)
    {
        return await _context.Games
            .Include(g => g.Developer)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Game> CreateAsync(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return await _context.Games
            .Include(g => g.Developer)
            .FirstAsync(g => g.Id == game.Id);
    }

    public async Task<bool> UpdateAsync(Game game)
    {
        var existingGame = await _context.Games
            .FirstOrDefaultAsync(g => g.Id == game.Id);

        if (existingGame == null)
            return false;

        existingGame.Title = game.Title;
        existingGame.ReleaseDate = game.ReleaseDate;
        existingGame.Price = game.Price;
        existingGame.DeveloperId = game.DeveloperId;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var game = await _context.Games
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
            return false;

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return true;
    }
}