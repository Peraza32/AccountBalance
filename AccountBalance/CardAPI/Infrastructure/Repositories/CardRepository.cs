using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;
using CardAPI.Infrastructure.Persistance;
using CardAPI.Infrastructure.Repositories.Interfaces;

namespace CardAPI.Infrastructure.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly CardDbContext _context;

        public CardRepository(CardDbContext context)
        {
            _context = context;
        }

        public async Task AddCardPayment(CardPaymentDTO payment)
        {
            try
            {
                _context.PaymentsTcs.Add(new PaymentsTc
                {
                    IdCard = payment.cardId,
                    MvDate = payment.paymentDate,
                    MvDescription = payment.description,
                    Amount = payment.amount,
                    IdState = 1
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {

                throw;
            }
            
        }

        public async Task AddCardPurchase(MovementsTc purchase)
        {
            _context.MovementsTcs.Add(purchase);

        }

        public Task<object> GetCardAsync(string cardId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetCardBalanceAsync(string cardId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetCardTransaccionAsync(string cardId)
        {
            throw new NotImplementedException();
        }
    }
}
