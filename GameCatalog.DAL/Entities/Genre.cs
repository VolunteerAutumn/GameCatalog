namespace GameCatalog.DAL.Entities;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Many-to-Many navigation
    public ICollection<Game> Games { get; set; } = new List<Game>();
}