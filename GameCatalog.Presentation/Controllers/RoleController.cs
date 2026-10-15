using GameCatalog.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    // GET: api/Role
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleService.GetAllAsync();
        return Ok(roles);
    }

    // GET: api/Role/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var role = await _roleService.GetByIdAsync(id);

        if (role is null)
            return NotFound(new { message = "Role not found." });

        return Ok(role);
    }

    // POST: api/Role
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "Role name is required." });
        }

        var result = await _roleService.CreateAsync(request.Name.Trim());

        if (!result.Succeeded)
        {
            return Conflict(new
            {
                errors = result.Errors.Select(error => error.Description)
            });
        }

        var role = await GetRoleByNameAsync(request.Name.Trim());

        return CreatedAtAction(
            nameof(GetById),
            new { id = role!.Id },
            role);
    }

    // PUT: api/Role/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        string id,
        [FromBody] RoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "Role name is required." });
        }

        var existingRole = await _roleService.GetByIdAsync(id);

        if (existingRole is null)
        {
            return NotFound(new { message = "Role not found." });
        }

        var result = await _roleService.UpdateAsync(id, request.Name.Trim());

        if (!result.Succeeded)
        {
            return Conflict(new
            {
                errors = result.Errors.Select(error => error.Description)
            });
        }

        return Ok(await _roleService.GetByIdAsync(id));
    }

    // DELETE: api/Role/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _roleService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new { message = "Role not found." });
        }

        return NoContent();
    }

    private async Task<Microsoft.AspNetCore.Identity.IdentityRole?>
        GetRoleByNameAsync(string name)
    {
        var roles = await _roleService.GetAllAsync();

        return roles.FirstOrDefault(role =>
            string.Equals(
                role.Name,
                name,
                StringComparison.OrdinalIgnoreCase));
    }

    public record RoleRequest(string Name);
}