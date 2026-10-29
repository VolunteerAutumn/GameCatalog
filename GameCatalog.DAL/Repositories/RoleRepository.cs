using GameCatalog.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameCatalog.DAL.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleRepository(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<List<IdentityRole>> GetAllAsync()
    {
        return await _roleManager.Roles.ToListAsync();
    }

    public async Task<IdentityRole?> GetByIdAsync(string id)
    {
        return await _roleManager.FindByIdAsync(id);
    }

    public async Task<IdentityResult> CreateAsync(string name)
    {
        if (await _roleManager.RoleExistsAsync(name))
        {
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = "DuplicateRole",
                    Description = "A role with this name already exists."
                });
        }

        return await _roleManager.CreateAsync(
            new IdentityRole(name));
    }

    public async Task<IdentityResult> UpdateAsync(
        string id,
        string name)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
        {
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = "RoleNotFound",
                    Description = "Role not found."
                });
        }

        var existingRole = await _roleManager.FindByNameAsync(name);

        if (existingRole != null && existingRole.Id != id)
        {
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = "DuplicateRole",
                    Description = "A role with this name already exists."
                });
        }

        role.Name = name;

        return await _roleManager.UpdateAsync(role);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
            return false;

        var result = await _roleManager.DeleteAsync(role);

        return result.Succeeded;
    }
}
