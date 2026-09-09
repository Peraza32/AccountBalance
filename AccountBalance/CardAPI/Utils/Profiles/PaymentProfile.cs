using AutoMapper;
using CardAPI.Application.Card.Command.Payment;
using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;

namespace CardAPI.Utils.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<addPaymentCommand, PaymentsTc>()
                .ForMember(dest => dest.IdCard, opt => opt.MapFrom(src => src.cardId))
                .ForMember(dest => dest.MvDate, opt => opt.MapFrom(src => src.paymentDate))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.amount))
                .ForMember(dest => dest.MvDescription, opt => opt.MapFrom(src => src.description));

            CreateMap<CardPaymentDTO, addPaymentCommand>();
        }
    }
}
