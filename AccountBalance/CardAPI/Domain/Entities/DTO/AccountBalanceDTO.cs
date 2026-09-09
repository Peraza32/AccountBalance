using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardAPI.Domain.Entities.DTO
{
    public class AccountBalanceDTO 
    {
        public string cardHolderName { get; set; }
        public string cardNumber { get; set; }

        public decimal currentCredit { get; set; }

        public decimal availableCredit { get; set; }
        public decimal creditLimit { get; set; }

        public List<PurchaseDTO> purchases { get; set; } = new List<PurchaseDTO>();
        public decimal totalActualMonthPurchases { get; set; }
        public decimal previousMonthPurchases { get; set; }

        public decimal bonificationInterest { get; set; }

        public decimal minimumPayment { get; set; } 
    }
}
