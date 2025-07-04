using AutoMapper;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.Payments;
using Masaafa.WebApi.Models.SalesOrders;

namespace Masaafa.WebApi.Mappers;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<CreatePaymentRequest, Payment>();
        CreateMap<UpdatePaymentRequest, Payment>();
        CreateMap<Payment, PaymentResponse>();
    }
}