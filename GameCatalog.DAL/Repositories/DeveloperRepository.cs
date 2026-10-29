using GameCatalog.DAL.Data;
using GameCatalog.DAL.Entities;
using GameCatalog.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameCatalog.DAL.Repositories;

public class DeveloperRepository : IDeveloperRepository
{
    private readonly GameCatalogDbContext _context;

    public DeveloperRepository(GameCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<List<Developer>> GetAllAsync()
    {
        return await _context.Developers
            .Include(d => d.Games)
            .ToListAsync();
    }

    public async Task<Developer?> GetByIdAsync(int id)
    {
        return await _context.Developers
            .Include(d => d.Games)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Developer> CreateAsync(Developer developer)
    {
        _context.Developers.Add(developer);
        await _context.SaveChangesAsync();

        return developer;
    }

    public async Task<bool> UpdateAsync(Developer developer)
    {
        var existingDeveloper = await _context.Developers
            .FirstOrDefaultAsync(d => d.Id == developer.Id);

        if (existingDeveloper == null)
            return false;

        existingDeveloper.Name = developer.Name;
        existingDeveloper.Country = developer.Country;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var developer = await _context.Developers
            .FirstOrDefaultAsync(d => d.Id == id);

        if (developer == null)
            return false;

        _context.Developers.Remove(developer);
        await _context.SaveChangesAsync();

        return true;
    }
}