using AutoMapper;
using CardAPI.Application.Card.Command.Purchase;
using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;

namespace CardAPI.Utils.Profiles
{
    public class PurchaseProfile:Profile
    {
        public PurchaseProfile() {
            CreateMap<AddPurchaseCommand, MovementsTc>()
                .ForMember(dest => dest.IdCard, opt => opt.MapFrom(src => src.cardId))
                .ForMember(dest => dest.MvDate, opt => opt.MapFrom(src => src.purchaseDate))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.price))
                .ForMember(dest => dest.MvDescription, opt => opt.MapFrom(src => src.description));

            CreateMap<PurchaseDTO, AddPurchaseCommand>();
        }
    }
}
