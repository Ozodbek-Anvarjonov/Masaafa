using AutoMapper;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.SalesOrders;

namespace Masaafa.WebApi.Mappers;

public class SalesOrdersMappingProfile : Profile
{
    public SalesOrdersMappingProfile()
    {
        CreateMap<CreateSalesOrderItemRequest, SalesOrderItem>();
        CreateMap<UpdateSalesOrderItemRequest, SalesOrderItem>();
        CreateMap<SalesOrderItem, SalesOrderItemResponse>();
        CreateMap<UpdateSalesOrderItemSendDate, SalesOrderItem>();
        CreateMap<UpdateSalesOrderItemReceiveDate, SalesOrderItem>();

        CreateMap<CreateSalesOrderRequest, SalesOrder>();
        CreateMap<UpdateSalesOrderRequest, SalesOrder>();
        CreateMap<SalesOrder, SalesOrderResponse>();
        CreateMap<UpdateSalesOrderApprovedRequest, SalesOrder>();
        CreateMap<UpdateSalesOrderRejectRequest, SalesOrder>();
        CreateMap<UpdateSalesOrderCancelRequest, SalesOrder>();
    }
}