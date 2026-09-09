namespace CardAPI.Domain.Entities.DTO
{
    public class PurchaseDTO
    {
        public Guid cardId { get; set; }
        public DateTime purchaseDate { get; set; } 
        public string description { get; set; }
        public  decimal price { get; set; }

    }

    public class NewPurchaseResultDTO
    {
        public int purchaseId { get; set; }
        public int Status { get; set; }
    }
}
