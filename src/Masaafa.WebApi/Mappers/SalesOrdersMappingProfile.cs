using AutoMapper;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.SalesOrders;
using Masaafa.WebApi.Models.TransferRequests;

namespace Masaafa.WebApi.Mappers;

public class SalesOrdersMappingProfile : Profile
{
    public SalesOrdersMappingProfile()
    {
        CreateMap<CreateSalesOrderItemRequest, SalesOrderItem>();
        CreateMap<UpdateSalesOrderItemRequest, SalesOrderItem>();
        CreateMap<SalesOrderItem, SalesOrderItemResponse>();

        CreateMap<CreateSalesOrderRequest, SalesOrder>();
        CreateMap<UpdateSalesOrderRequest, SalesOrder>();
        CreateMap<SalesOrder, SalesOrderResponse>();
    }
}