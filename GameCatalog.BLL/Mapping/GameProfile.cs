using AutoMapper;
using GameCatalog.BLL.DTOs;
using GameCatalog.DAL.Entities;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<GameCreateDto, Game>();
        CreateMap<GameUpdateDto, Game>();

        CreateMap<Game, GameResponseDto>()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres));

        CreateMap<Developer, DeveloperShortDto>();
        CreateMap<Genre, GenreDto>();
    }
}