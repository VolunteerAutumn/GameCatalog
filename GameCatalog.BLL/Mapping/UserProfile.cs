using AutoMapper;
using GameCatalog.BLL.DTOs;
using GameCatalog.DAL.Entities;

namespace GameCatalog.BLL.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserProfileDto>();
    }
}