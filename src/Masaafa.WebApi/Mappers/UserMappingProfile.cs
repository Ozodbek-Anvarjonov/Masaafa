using AutoMapper;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.Users;

namespace Masaafa.WebApi.Mappers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateClientRequest, Client>();
        CreateMap<UpdateClientRequest, Client>();
        CreateMap<Client, ClientResponse>();

        CreateMap<CreateEmployeeRequest, Employee>();
        CreateMap<UpdateEmployeeRequest, Employee>();
        CreateMap<Employee, EmployeeResponse>();
    }
}