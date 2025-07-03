using AutoMapper;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.Items;

namespace Masaafa.WebApi.Mappers;

public class ItemsMapperProfile : Profile
{
    public ItemsMapperProfile()
    {
        CreateMap<CreateItemGroupRequest, ItemGroup>();
        CreateMap<UpdateItemGroupRequest, ItemGroup>();
        CreateMap<ItemGroup, ItemGroupResponse>();

        CreateMap<CreateItemRequest, Item>();
        CreateMap<UpdateItemRequest, Item>();
        CreateMap<Item, ItemResponse>();
    }
}