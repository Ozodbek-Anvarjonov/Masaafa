using AutoMapper;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.Warehouses;

namespace Masaafa.WebApi.Mappers;

public class WarehousesMappingProfile : Profile
{
    public WarehousesMappingProfile()
    {
        CreateMap<CreateWarehouseItemRequest, WarehouseItem>();
        CreateMap<UpdateWarehouseItemRequest, WarehouseItem>();
        CreateMap<WarehouseItem, WarehouseItemResponse>();

        CreateMap<CreateWarehouseRequest, Warehouse>();
        CreateMap<UpdateWarehouseRequest, Warehouse>();
        CreateMap<Warehouse, WarehouseResponse>();
    }
}