namespace GameCatalog.BLL.DTOs;

public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class GenreCreateDto
{
    public string Name { get; set; } = string.Empty;
}

public class GenreUpdateDto
{
    public string Name { get; set; } = string.Empty;
}