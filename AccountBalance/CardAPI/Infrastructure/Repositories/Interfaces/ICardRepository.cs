using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;

namespace CardAPI.Infrastructure.Repositories.Interfaces
{
    public interface ICardRepository
    {
        public Task<object> GetCardAsync(string cardId);
        public Task<object> GetCardTransaccionAsync(string cardId);
        public Task<object> GetCardBalanceAsync(string cardId);
        public Task<NewPurchaseResultDTO> AddCardPurchase(MovementsTc purchase );

        public Task AddCardPayment(PaymentsTc payment);

    }
}
