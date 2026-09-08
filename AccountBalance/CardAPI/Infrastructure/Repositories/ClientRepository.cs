using CardAPI.Domain.Entities.DTO;
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

        public async Task<List<UserWithCardsDTO>> GetUserAndCards(string user)
        {
            var clientIdParam = new SqlParameter("@CLIENT", user);
            var result = await _context.UserWithCardsDTO
                .FromSqlInterpolated($"EXEC GetUserAndCards @CLIENT={clientIdParam}")
                .AsNoTracking()
                .ToListAsync();

            return result;
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
                    state = m.IdState
                })
                .ToListAsync();
            return purchases;
        }
    }
}
