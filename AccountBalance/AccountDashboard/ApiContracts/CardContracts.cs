namespace AccountDashboard.Web.ApiContracts
{
    // -------------------------------------------------------------------
    // Contratos para CardController (CardAPI backend).
    // -------------------------------------------------------------------

    
    public class PurchaseRequestDto
    {
        public Guid cardId { get; set; }
        public DateTime purchaseDate { get; set; }
        public string description { get; set; } = string.Empty;
        public decimal price { get; set; }
    }

 
    public class PaymentRequestDto
    {
        public Guid cardId { get; set; }
        public DateTime paymentDate { get; set; }
        public string description { get; set; } = string.Empty;
        public decimal amount { get; set; }
    }

    
    public class PurchaseResultDto
    {
        public int purchaseId { get; set; }
        public int Status { get; set; }
    }

    public class PaymentResultDto
    {
        public int paymentId { get; set; }
    }
}
