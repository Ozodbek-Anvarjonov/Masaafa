using AutoMapper;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.TransferRequests;

namespace Masaafa.WebApi.Mappers;

public class TransfersMappingProfile : Profile
{
    public TransfersMappingProfile()
    {
        CreateMap<CreateTransferItemRequest, TransferRequestItem>();
        CreateMap<UpdateTransferItemRequest, TransferRequestItem>();
        CreateMap<TransferRequestItem, TransferItemResponse>();
        CreateMap<UpdateTransferItemSentDate, TransferRequestItem>();
        CreateMap<UpdateTransferItemReceiveDate, TransferRequestItem>();

        CreateMap<CreateTransferRequest, TransferRequest>();
        CreateMap<UpdateTransferRequest, TransferRequest>();
        CreateMap<TransferRequest, TransferResponse>();
        CreateMap<UpdateTransferApproveRequest, TransferRequest>();
        CreateMap<UpdateTransferRejectRequest, TransferRequest>();
    }
}