using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;
using CardAPI.Infrastructure.Persistance;
using CardAPI.Infrastructure.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CardAPI.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly CardDbContext _context;

        public ClientRepository(CardDbContext context)
        {
            _context = context;
        }

        public async Task<UserWithCardsDTO> GetUserAndCard(string user)
        {
            var clientIdParam = new SqlParameter("@CLIENT", user);
            var result = await _context.UserWithCards
                .FromSqlInterpolated($"EXEC GET_CLIENTWITHCARDS @CLIENT={clientIdParam}")
                .AsNoTracking()
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<List<PurchaseDTO>> GetUserPurchases(int userId, Guid cardId)
        {
            var purchases = await _context.MovementsTcs
                .Where(m => m.IdCard == cardId)
                .Select(m => new PurchaseDTO
                {
                    cardId = m.IdCard,
                    purchaseDate = m.MvDate,
                    description = m.MvDescription,
                    price = m.Amount,
                })
                .ToListAsync();
            return purchases;
        }

        public async Task<List<TransactionHistoryDTO>> GetCurrentMonthTransactionsAsync(Guid cardId, string userId)
        {
            var now = DateTime.UtcNow;

            var purchases = _context.MovementsTcs
                .Where(m => m.IdCard == cardId
                         && m.MvDate.Year == now.Year
                         && m.MvDate.Month == now.Month)
                .Select(m => new TransactionHistoryDTO
                {
                    Date = m.MvDate,
                    Description = m.MvDescription,
                    Amount = m.Amount,
                    Type = "COMPRA",
                    State = m.IdState
                });

            var payments = _context.PaymentsTcs
                .Where(p => p.IdCard == cardId
                         && p.MvDate.Year == now.Year
                         && p.MvDate.Month == now.Month)
                .Select(p => new TransactionHistoryDTO
                {
                    Date = p.MvDate,
                    Description = p.MvDescription,
                    Amount = p.Amount,
                    Type = "PAGO",
                    State = p.IdState
                });

            var result = await purchases
                .Union(payments)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            return result;
        }

        public async Task<UserWithCardsDTO> GetUserByName(string userName)
        {
            var clientIdParam = new SqlParameter("@CLIENT_NAMET", userName);
            var result = await _context.UserWithCards
                .FromSqlInterpolated($"EXEC GET_CLIENTWITHCARDSBYNAME @CLIENT_NAME={clientIdParam}")
                .AsNoTracking()
                .ToListAsync();

            return result.FirstOrDefault();
        }
    }
}
