using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Entities;
using GameCatalog.Presentation.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeveloperController : ControllerBase
{
    private readonly IDeveloperService _service;

    public DeveloperController(IDeveloperService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<DeveloperResponseDto>>> GetAll()
    {
        var developers = await _service.GetAllAsync();

        var result = developers.Select(d => new DeveloperResponseDto
        {
            Id = d.Id,
            Name = d.Name,
            Country = d.Country,
            Games = d.Games.Select(g => new GameShortDto
            {
                Id = g.Id,
                Title = g.Title,
                ReleaseDate = g.ReleaseDate,
                Price = g.Price,
                DeveloperId = g.DeveloperId
            }).ToList()
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeveloperResponseDto>> GetById(int id)
    {
        var developer = await _service.GetByIdAsync(id);

        if (developer == null)
            return NotFound();

        var result = new DeveloperResponseDto
        {
            Id = developer.Id,
            Name = developer.Name,
            Country = developer.Country,
            Games = developer.Games.Select(g => new GameShortDto
            {
                Id = g.Id,
                Title = g.Title,
                ReleaseDate = g.ReleaseDate,
                Price = g.Price,
                DeveloperId = g.DeveloperId
            }).ToList()
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Developer>> Create(Developer developer)
    {
        var createdDeveloper = await _service.CreateAsync(developer);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdDeveloper.Id },
            createdDeveloper);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Developer developer)
    {
        if (id != developer.Id)
            return BadRequest();

        var updated = await _service.UpdateAsync(developer);

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