using System.Text.Json.Serialization;

namespace GameCatalog.DAL.Entities;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public decimal Price { get; set; }

    public int DeveloperId { get; set; }
    public Developer? Developer { get; set; }

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
}