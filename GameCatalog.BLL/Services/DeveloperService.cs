using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Entities;
using GameCatalog.DAL.Interfaces;

namespace GameCatalog.BLL.Services;

public class DeveloperService : IDeveloperService
{
    private readonly IDeveloperRepository _repository;

    public DeveloperService(IDeveloperRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Developer>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Developer?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Developer> CreateAsync(Developer developer)
    {
        return await _repository.CreateAsync(developer);
    }

    public async Task<bool> UpdateAsync(Developer developer)
    {
        return await _repository.UpdateAsync(developer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}