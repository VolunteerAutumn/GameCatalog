namespace GameCatalog.BLL.DTOs;

public class DeveloperResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public List<GameShortDto> Games { get; set; } = new();
}