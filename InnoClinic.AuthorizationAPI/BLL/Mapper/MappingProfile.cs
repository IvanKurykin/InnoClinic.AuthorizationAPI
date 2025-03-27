using BLL.Models.DTOs;
using DAL.Entities;
using AutoMapper;

namespace BLL.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDTO, User>();
        CreateMap<User, RegisterDTO>();
        CreateMap<LogInDTO, User>();
        CreateMap<User, LogInDTO>();
    }
}
