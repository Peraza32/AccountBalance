using AutoMapper;
using AccountDashboard.Web.ApiContracts;
using AccountDashboard.Web.Common;
using AccountDashboard.Web.Models;

namespace AccountDashboard.Web.Mapping
{
    
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ---- Respuestas de la API -> ViewModels ----
            CreateMap<AccountBalanceResponseDto, AccountStatementViewModel>()
                .ForMember(d => d.CardHolderName, o => o.MapFrom(s => s.cardHolderName))
                .ForMember(d => d.CardNumberMasked, o => o.MapFrom(s => CardMasking.FormatCardNumber(s.cardNumber)))
                .ForMember(d => d.CurrentCredit, o => o.MapFrom(s => s.currentCredit))
                .ForMember(d => d.AvailableCredit, o => o.MapFrom(s => s.availableCredit))
                .ForMember(d => d.CreditLimit, o => o.MapFrom(s => s.creditLimit))
                .ForMember(d => d.Purchases, o => o.MapFrom(s => s.purchases))
                .ForMember(d => d.TotalActualMonthPurchases, o => o.MapFrom(s => s.totalActualMonthPurchases))
                .ForMember(d => d.PreviousMonthPurchases, o => o.MapFrom(s => s.previousMonthPurchases))
                .ForMember(d => d.BonificationInterest, o => o.MapFrom(s => s.bonificationInterest))
                .ForMember(d => d.MinimumPayment, o => o.MapFrom(s => s.minimumPayment))
                .ForMember(d => d.TotalWithInterest, o => o.MapFrom(s => s.totalWithInterest));

            CreateMap<PurchaseLineDto, PurchaseLineViewModel>()
                .ForMember(d => d.PurchaseDate, o => o.MapFrom(s => s.purchaseDate))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.description))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.price));

            CreateMap<ClientWithCardDto, ClientCardOptionViewModel>()
                .ForMember(d => d.CardId, o => o.MapFrom(s => s.idCard.ToString()))
                .ForMember(d => d.CardNumberMasked, o => o.MapFrom(s => CardMasking.FormatCardNumber(s.cardNumber)));

            CreateMap<TransactionHistoryLineDto, TransactionItemViewModel>()
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Type))
                .ForMember(d => d.State, o => o.MapFrom(s => s.State));

            // ---- ViewModels (formularios) -> Requests de la API ----
            CreateMap<PurchaseFormViewModel, PurchaseRequestDto>()
                .ForMember(d => d.cardId, o => o.Ignore()) // se asigna explicitamente desde Session (dato sensible)
                .ForMember(d => d.purchaseDate, o => o.MapFrom(s => s.PurchaseDate))
                .ForMember(d => d.description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.price, o => o.MapFrom(s => s.Price));

            CreateMap<PaymentFormViewModel, PaymentRequestDto>()
                .ForMember(d => d.cardId, o => o.Ignore()) // se asigna explicitamente desde Session (dato sensible)
                .ForMember(d => d.paymentDate, o => o.MapFrom(s => s.PaymentDate))
                .ForMember(d => d.description, o => o.MapFrom(s => s.Description ?? string.Empty))
                .ForMember(d => d.amount, o => o.MapFrom(s => s.Amount));
        }
    }
}
