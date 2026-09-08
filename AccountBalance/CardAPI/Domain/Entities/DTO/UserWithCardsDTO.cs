namespace CardAPI.Domain.Entities.DTO
{
    public class UserWithCardsDTO
    {
        public string clientId { get; set; }
        public string clientName { get; set; }
        public string cardNumber { get; set; } = string.Empty;

    }
}
