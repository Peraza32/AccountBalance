using CardAPI.Domain.Entities.DTO;

namespace CardAPI.Infrastructure.Repositories.Interfaces
{
    public interface IClientRepository
    {
        public Task<List<UserWithCardsDTO>> GetUserAndCards(string user);
        public Task<List<PurchaseDTO>> GetUserPurchases(int userId, Guid cardId);
    }
}
