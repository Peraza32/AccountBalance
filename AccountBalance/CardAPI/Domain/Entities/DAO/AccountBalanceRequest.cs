namespace CardAPI.Domain.Entities.DAO
{
    public class AccountBalanceRequest
    {
        public Guid cardId { get; set; }
        public string userId { get; set; }
    }
}
