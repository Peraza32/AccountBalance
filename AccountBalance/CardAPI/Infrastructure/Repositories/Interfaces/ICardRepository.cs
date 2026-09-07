namespace CardAPI.Infrastructure.Repositories.Interfaces
{
    public interface ICardRepository
    {
        public Task<object> GetCardAsync(string cardId);
        public Task<object> GetCardTransaccionAsync(string cardId);
        public Task<object> GetCardBalanceAsync(string cardId);
        public Task<object> AddCardPurchase(string cardId, decimal amount);
        public Task<object> AddCardPayment(string cardId, decimal amount);

    }
}
