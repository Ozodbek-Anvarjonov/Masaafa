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

        CreateMap<CreateTransferRequest, TransferRequest>();
        CreateMap<UpdateTransferRequest, TransferRequest>();
        CreateMap<TransferRequest, TransferResponse>();
    }
}