using System.Text.Json.Serialization;

namespace GameCatalog.DAL.Entities;

public class Developer
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Game> Games { get; set; } = new List<Game>();
}