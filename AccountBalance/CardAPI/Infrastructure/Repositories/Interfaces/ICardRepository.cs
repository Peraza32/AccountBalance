using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;

namespace CardAPI.Infrastructure.Repositories.Interfaces
{
    public interface ICardRepository
    {
        public Task<object> GetCardAsync(string cardId);
        public Task<AccountBalanceDTO> GetCardBalanceAsync(Guid cardId);
        public Task<NewPurchaseResultDTO> AddCardPurchase(MovementsTc purchase );

        public Task<PaymentResponseDTO> AddCardPayment(PaymentsTc payment);

    }
}
