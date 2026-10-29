using AutoMapper;
using GameCatalog.BLL.DTOs;
using GameCatalog.DAL.Entities;

namespace GameCatalog.BLL.Mapping;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<GenreCreateDto, Genre>();
        CreateMap<GenreUpdateDto, Genre>();
        CreateMap<Genre, GenreDto>();
    }
}