using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace GameCatalog.BLL.Validators;

public class RoleValidator : IRoleValidator<IdentityRole>
{
    // Start with a letter, allow letters, digits, spaces, hyphens and underscores.
    // Length between 3 and 100.
    private static readonly Regex AllowedName =
        new(@"^[A-Za-z][A-Za-z0-9 _-]{2,99}$", RegexOptions.Compiled);

    public Task<IdentityResult> ValidateAsync(
        RoleManager<IdentityRole> manager,
        IdentityRole role)
    {
        var errors = new List<IdentityError>();

        if (role == null || string.IsNullOrWhiteSpace(role.Name))
        {
            errors.Add(new IdentityError
            {
                Code = "InvalidName",
                Description = "Role name is required."
            });

            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }

        var name = role.Name.Trim();

        if (name.Length < 3 || name.Length > 100)
        {
            errors.Add(new IdentityError
            {
                Code = "InvalidLength",
                Description = "Role name must be between 3 and 100 characters."
            });
        }

        if (!AllowedName.IsMatch(name))
        {
            errors.Add(new IdentityError
            {
                Code = "InvalidCharacters",
                Description = "Role name must start with a letter and contain only letters, digits, spaces, hyphens or underscores."
            });
        }

        if (errors.Any())
            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));

        return Task.FromResult(IdentityResult.Success);
    }
}