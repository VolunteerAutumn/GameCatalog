using Microsoft.AspNetCore.Identity;

namespace GameCatalog.BLL.Interfaces;

public interface IRoleService
{
    Task<List<IdentityRole>> GetAllAsync();

    Task<IdentityRole?> GetByIdAsync(string id);

    Task<IdentityResult> CreateAsync(string name);

    Task<IdentityResult> UpdateAsync(string id, string name);

    Task<bool> DeleteAsync(string id);
}
