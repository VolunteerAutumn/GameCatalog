using Microsoft.AspNetCore.Identity;

namespace GameCatalog.DAL.Entities;

public class ApplicationUser : IdentityUser
{
    public DateTime? DateOfBirth { get; set; }
    public bool IsSubscribedToNewsletter { get; set; } = false;
}