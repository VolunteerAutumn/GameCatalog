namespace GameCatalog.BLL.DTOs;

public class GameUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public decimal Price { get; set; }
    public int DeveloperId { get; set; }
}