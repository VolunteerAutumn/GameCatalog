namespace GameCatalog.BLL.DTOs;

public class UserProfileDto
{
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public bool IsSubscribedToNewsletter { get; set; }
}

public class UpdateProfileDto
{
    public string UserName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public bool IsSubscribedToNewsletter { get; set; }
}