using AutoMapper;
using GameCatalog.BLL.DTOs;
using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Entities;
using GameCatalog.DAL.Interfaces;

namespace GameCatalog.BLL.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _repository;
    private readonly IMapper _mapper;

    public GenreService(IGenreRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<GenreDto>> GetAllAsync()
    {
        var genres = await _repository.GetAllAsync();
        return _mapper.Map<List<GenreDto>>(genres);
    }

    public async Task<GenreDto?> GetByIdAsync(int id)
    {
        var genre = await _repository.GetByIdAsync(id);
        return genre == null ? null : _mapper.Map<GenreDto>(genre);
    }

    public async Task<GenreDto> CreateAsync(GenreCreateDto dto)
    {
        var genre = _mapper.Map<Genre>(dto);
        var created = await _repository.CreateAsync(genre);
        return _mapper.Map<GenreDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, GenreUpdateDto dto)
    {
        var genre = _mapper.Map<Genre>(dto);
        genre.Id = id;
        return await _repository.UpdateAsync(genre);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}