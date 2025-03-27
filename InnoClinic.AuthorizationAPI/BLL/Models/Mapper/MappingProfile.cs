using BLL.Models.DTOs;
using DAL.Entities;
using AutoMapper;

namespace BLL.Models.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDTO, User>().ReverseMap();
        CreateMap<LogInDTO, User>().ReverseMap();
    }
}
