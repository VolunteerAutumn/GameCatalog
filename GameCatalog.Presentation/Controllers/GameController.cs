using AutoMapper;
using FluentValidation;
using GameCatalog.BLL.DTOs;
using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GameController : ControllerBase
{
    private readonly IGameService _service;
    private readonly IMapper _mapper;
    private readonly IValidator<GameCreateDto> _createValidator;
    private readonly IValidator<GameUpdateDto> _updateValidator;

    public GameController(
        IGameService service,
        IMapper mapper,
        IValidator<GameCreateDto> createValidator,
        IValidator<GameUpdateDto> updateValidator)
    {
        _service = service;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<GameResponseDto>>> GetAll()
    {
        var games = await _service.GetAllAsync();
        var result = _mapper.Map<List<GameResponseDto>>(games);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameResponseDto>> GetById(int id)
    {
        var game = await _service.GetByIdAsync(id);
        if (game == null) return NotFound();

        var result = _mapper.Map<GameResponseDto>(game);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<GameResponseDto>> Create(GameCreateDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var game = _mapper.Map<Game>(dto);
        var createdGame = await _service.CreateAsync(game);
        var result = _mapper.Map<GameResponseDto>(createdGame);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, GameUpdateDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var game = _mapper.Map<Game>(dto);
        game.Id = id;

        var updated = await _service.UpdateAsync(game);
        if (!updated) return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}