using AutoMapper;
using GameCatalog.BLL.DTOs;
using GameCatalog.DAL.Entities;

namespace GameCatalog.BLL.Mapping;

public class DeveloperProfile : Profile
{
    public DeveloperProfile()
    {
        CreateMap<DeveloperCreateDto, Developer>();

        CreateMap<DeveloperUpdateDto, Developer>();

        CreateMap<Developer, DeveloperResponseDto>();

        CreateMap<Game, GameShortDto>();
    }
}