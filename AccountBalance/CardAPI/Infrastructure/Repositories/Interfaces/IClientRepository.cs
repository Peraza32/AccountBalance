namespace CardAPI.Infrastructure.Repositories.Interfaces
{
    public interface IClientRepository
    {
        public Task<object> GetUserAndCards(string user);
        public Task<object> GetUserPurchases(int userId, int cardId);
    }
}
