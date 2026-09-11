namespace AccountDashboard.Web.ApiContracts
{
    // -------------------------------------------------------------------
    // Contratos para ClientController (CardAPI backend).
    // -------------------------------------------------------------------

   
    public class AccountBalanceRequestDto
    {
        public Guid cardId { get; set; }
        public string userId { get; set; } = string.Empty;
    }

  
    public class AccountBalanceResponseDto
    {
        public string cardHolderName { get; set; } = string.Empty;
        public string cardNumber { get; set; } = string.Empty;
        public decimal currentCredit { get; set; }
        public decimal availableCredit { get; set; }
        public decimal creditLimit { get; set; }
        public List<PurchaseLineDto> purchases { get; set; } = new();
        public decimal totalActualMonthPurchases { get; set; }
        public decimal previousMonthPurchases { get; set; }
        public decimal bonificationInterest { get; set; }
        public decimal minimumPayment { get; set; }
        public decimal totalWithInterest { get; set; }
    }

    
    public class PurchaseLineDto
    {
        public Guid cardId { get; set; }
        public DateTime purchaseDate { get; set; }
        public string description { get; set; } = string.Empty;
        public decimal price { get; set; }
    }

   
    public class ClientWithCardDto
    {
        public string clientId { get; set; } = string.Empty;
        public string clientName { get; set; } = string.Empty;
        public string cardNumber { get; set; } = string.Empty;
        public Guid idCard { get; set; }
    }

    
    public class TransactionHistoryRequestDto
    {
        public Guid CardId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

   
    public class TransactionHistoryResponseDto
    {
        
        public List<TransactionHistoryLineDto> Transactions { get; set; } = new();
    }

    
    public class TransactionHistoryLineDto
    {
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public int State { get; set; }
    }
}
