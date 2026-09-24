namespace GameCatalog.Presentation.DTOs;

public class GameShortDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime ReleaseDate { get; set; }

    public decimal Price { get; set; }

    public int DeveloperId { get; set; }
}