using CardAPI.Application.Client.Queries.AccountBalance;
using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;

namespace CardAPI.Infrastructure.Repositories.Interfaces
{
    public interface ICardRepository
    {
        public Task<object> GetCardAsync(string cardId);
        public Task<AccountBalanceResponseDTO> GetCardBalanceAsync(GetAccountBalance request);
        public Task<NewPurchaseResultDTO> AddCardPurchase(MovementsTc purchase );

        public Task<PaymentResponseDTO> AddCardPayment(PaymentsTc payment);

    }
}
