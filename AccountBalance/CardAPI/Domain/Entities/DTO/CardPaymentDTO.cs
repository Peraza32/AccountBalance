namespace CardAPI.Domain.Entities.DTO
{
    public class CardPaymentDTO
    {
        public Guid cardId { get; set; }
        public DateTime paymentDate { get; set; } = DateTime.Now;
        public string description { get; set; } 
        public decimal amount { get; set; }
    }
}
