using CardAPI.Domain.Entities.DTO;

namespace CardAPI.Infrastructure.Repositories.Interfaces
{
    public interface IPurchaseRepository
    {
        public Task<List<PurchaseDTO>> GetPurchasesCurrentMonthAsync(string cardId);
    }
}
