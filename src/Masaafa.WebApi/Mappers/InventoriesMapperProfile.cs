using AutoMapper;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.Inventories;
using Masaafa.WebApi.Models.Users;
using Masaafa.WebApi.Models.Warehouses;

namespace Masaafa.WebApi.Mappers;

public class InventoriesMapperProfile : Profile
{
    public InventoriesMapperProfile()
    {
        CreateMap<CreateInventoryRequest, Inventory>();
        CreateMap<UpdateInventoryRequest, Inventory>();
        CreateMap<Inventory, InventoryResponse>()
            .ForMember(dest => dest.InventoryDate, opt => opt.MapFrom(src => src.InventoryDate.DateTime))
            .ForMember(dest => dest.StartedDate, opt => opt.MapFrom(src => src.StartedDate.HasValue ? src.StartedDate.Value.DateTime : (DateTime?)null))
            .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate.HasValue ? src.CompletedDate.Value.DateTime : (DateTime?)null));

        CreateMap<CreateInventoryItemRequest, InventoryItem>();
        CreateMap<UpdateInventoryItemRequest, InventoryItem>();
        CreateMap<InventoryItem, InventoryItemResponse>();
    }
}