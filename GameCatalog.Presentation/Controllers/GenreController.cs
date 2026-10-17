using FluentValidation;
using GameCatalog.BLL.DTOs;
using GameCatalog.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenreController : ControllerBase
{
    private readonly IGenreService _service;
    private readonly IValidator<GenreCreateDto> _createValidator;
    private readonly IValidator<GenreUpdateDto> _updateValidator;

    public GenreController(
        IGenreService service,
        IValidator<GenreCreateDto> createValidator,
        IValidator<GenreUpdateDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<GenreDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GenreDto>> GetById(int id)
    {
        var genre = await _service.GetByIdAsync(id);
        if (genre == null) return NotFound();
        return Ok(genre);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GenreDto>> Create(GenreCreateDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, GenreUpdateDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var updated = await _service.UpdateAsync(id, dto);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}