using GameCatalog.DAL.Data;
using GameCatalog.DAL.Entities;
using GameCatalog.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameCatalog.DAL.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly GameCatalogDbContext _context;

    public GenreRepository(GameCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<List<Genre>> GetAllAsync()
    {
        return await _context.Genres.Include(g => g.Games).ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        return await _context.Genres.Include(g => g.Games).FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Genre> CreateAsync(Genre genre)
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task<bool> UpdateAsync(Genre genre)
    {
        var existing = await _context.Genres.FirstOrDefaultAsync(g => g.Id == genre.Id);
        if (existing == null) return false;

        existing.Name = genre.Name;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null) return false;

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
        return true;
    }
}