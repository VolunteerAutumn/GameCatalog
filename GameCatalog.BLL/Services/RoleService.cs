using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GameCatalog.BLL.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;

    public RoleService(IRoleRepository repository)
    {
        _repository = repository;
    }

    public Task<List<IdentityRole>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<IdentityRole?> GetByIdAsync(string id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<IdentityResult> CreateAsync(string name)
    {
        return _repository.CreateAsync(name);
    }

    public Task<IdentityResult> UpdateAsync(string id, string name)
    {
        return _repository.UpdateAsync(id, name);
    }

    public Task<bool> DeleteAsync(string id)
    {
        return _repository.DeleteAsync(id);
    }
}
