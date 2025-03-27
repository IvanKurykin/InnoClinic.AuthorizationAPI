using BLL.DTO;
using DAL.Entities;
using AutoMapper;

namespace BLL.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDto, User>().ReverseMap();
        CreateMap<LogInDto, User>().ReverseMap();
    }
}
