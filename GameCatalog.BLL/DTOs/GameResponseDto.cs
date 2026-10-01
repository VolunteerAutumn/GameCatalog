namespace GameCatalog.BLL.DTOs;

public class GameResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime ReleaseDate { get; set; }

    public decimal Price { get; set; }

    public int DeveloperId { get; set; }

    public DeveloperShortDto? Developer { get; set; }
}

public class DeveloperShortDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;
}