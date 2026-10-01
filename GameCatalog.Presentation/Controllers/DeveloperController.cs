using AutoMapper;
using FluentValidation;
using GameCatalog.BLL.DTOs;
using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeveloperController : ControllerBase
{
    private readonly IDeveloperService _service;
    private readonly IMapper _mapper;
    private readonly IValidator<DeveloperCreateDto> _createValidator;
    private readonly IValidator<DeveloperUpdateDto> _updateValidator;

    public DeveloperController(
        IDeveloperService service,
        IMapper mapper,
        IValidator<DeveloperCreateDto> createValidator,
        IValidator<DeveloperUpdateDto> updateValidator)
    {
        _service = service;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<DeveloperResponseDto>>> GetAll()
    {
        var developers = await _service.GetAllAsync();

        var result = _mapper.Map<List<DeveloperResponseDto>>(developers);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeveloperResponseDto>> GetById(int id)
    {
        var developer = await _service.GetByIdAsync(id);

        if (developer == null)
            return NotFound();

        var result = _mapper.Map<DeveloperResponseDto>(developer);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<DeveloperResponseDto>> Create(
        DeveloperCreateDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var developer = _mapper.Map<Developer>(dto);

        var createdDeveloper = await _service.CreateAsync(developer);

        var result = _mapper.Map<DeveloperResponseDto>(createdDeveloper);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        DeveloperUpdateDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var developer = _mapper.Map<Developer>(dto);

        developer.Id = id;

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