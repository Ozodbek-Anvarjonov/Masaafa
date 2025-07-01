using AutoMapper;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.Users;

namespace Masaafa.WebApi.Mappers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserRequest, User>();
        CreateMap<UpdateUserRequest, User>();
        CreateMap<User, UserResponse>();
    }
}