using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Entities;
using GameCatalog.BLL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _service;

    public GameController(IGameService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<GameResponseDto>>> GetAll()
    {
        var games = await _service.GetAllAsync();

        var result = games.Select(g => new GameResponseDto
        {
            Id = g.Id,
            Title = g.Title,
            ReleaseDate = g.ReleaseDate,
            Price = g.Price,
            DeveloperId = g.DeveloperId,
            Developer = g.Developer == null
                ? null
                : new DeveloperShortDto
                {
                    Id = g.Developer.Id,
                    Name = g.Developer.Name,
                    Country = g.Developer.Country
                }
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameResponseDto>> GetById(int id)
    {
        var game = await _service.GetByIdAsync(id);

        if (game == null)
            return NotFound();

        var result = new GameResponseDto
        {
            Id = game.Id,
            Title = game.Title,
            ReleaseDate = game.ReleaseDate,
            Price = game.Price,
            DeveloperId = game.DeveloperId,
            Developer = game.Developer == null
                ? null
                : new DeveloperShortDto
                {
                    Id = game.Developer.Id,
                    Name = game.Developer.Name,
                    Country = game.Developer.Country
                }
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<GameResponseDto>> Create(Game game)
    {
        var createdGame = await _service.CreateAsync(game);

        var result = new GameResponseDto
        {
            Id = createdGame.Id,
            Title = createdGame.Title,
            ReleaseDate = createdGame.ReleaseDate,
            Price = createdGame.Price,
            DeveloperId = createdGame.DeveloperId,
            Developer = createdGame.Developer == null
                ? null
                : new DeveloperShortDto
                {
                    Id = createdGame.Developer.Id,
                    Name = createdGame.Developer.Name,
                    Country = createdGame.Developer.Country
                }
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Game game)
    {
        if (id != game.Id)
            return BadRequest();

        var updated = await _service.UpdateAsync(game);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}