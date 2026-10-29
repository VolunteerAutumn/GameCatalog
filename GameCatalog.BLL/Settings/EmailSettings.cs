namespace GameCatalog.BLL.Settings;

public class EmailSettings
{
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = "GameCatalog";
    public string Password { get; set; } = string.Empty;

    public bool UseFakeSender { get; set; } = false;
}