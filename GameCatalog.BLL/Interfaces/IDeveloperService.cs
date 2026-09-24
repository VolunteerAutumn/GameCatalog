using GameCatalog.DAL.Entities;

namespace GameCatalog.BLL.Interfaces;

public interface IDeveloperService
{
    Task<List<Developer>> GetAllAsync();

    Task<Developer?> GetByIdAsync(int id);

    Task<Developer> CreateAsync(Developer developer);

    Task<bool> UpdateAsync(Developer developer);

    Task<bool> DeleteAsync(int id);
}