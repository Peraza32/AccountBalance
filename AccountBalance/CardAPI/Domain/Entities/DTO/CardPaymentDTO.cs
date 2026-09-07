namespace CardAPI.Domain.Entities.DTO
{
    public class CardPaymentDTO
    {
        public Guid cardId { get; set; }
        public DateTime paymentDate { get; set; } = DateTime.Now;
        public decimal amount { get; set; }

        public int state { get; set; } = 1;
    }
}
