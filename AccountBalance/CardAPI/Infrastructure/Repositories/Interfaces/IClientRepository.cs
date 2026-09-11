using CardAPI.Domain.Entities.DTO;

namespace CardAPI.Infrastructure.Repositories.Interfaces
{
    public interface IClientRepository
    {
        public Task<UserWithCardsDTO> GetUserAndCard(string user);
        public Task<List<PurchaseDTO>> GetUserPurchases(int userId, Guid cardId);
    }
}
