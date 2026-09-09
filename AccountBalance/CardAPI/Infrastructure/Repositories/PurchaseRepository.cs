using CardAPI.Domain.Entities.DTO;
using CardAPI.Infrastructure.Persistance;
using CardAPI.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CardAPI.Infrastructure.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly CardDbContext _context;
        private readonly ILogsRepository _logsRepository;

        public PurchaseRepository(CardDbContext context, ILogsRepository logsRepository)
        {
            _context = context;
            _logsRepository = logsRepository;
        }

        public async Task<List<PurchaseDTO>> GetPurchasesCurrentMonthAsync(string cardId)
        {
            try 
            { 
                DateTime now = DateTime.Now;

                var purchases = await _context.MovementsTcs
                    .Where(m => m.IdCard.ToString() == cardId && 
                    m.MvDate.Month == now.Month && 
                    m.MvDate.Year == now.Year &&
                    m.IdState == 3)
                    .OrderByDescending(m => m.MvDate)
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
            catch (Exception ex)
            {
                await _logsRepository.InsertLogAsync("PurchaseRepository", ex.Message, DateTime.Now);
                throw;
            }
        }
    }
}
