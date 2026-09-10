using System;
using System.Collections.Generic;

namespace CardAPI.Domain.Models
{
    public partial class PaymentsTc
    {
        public int Id { get; set; }
        public DateTime MvDate { get; set; }
        public Guid IdCard { get; set; }
        public decimal Amount { get; set; }
        public string MvDescription { get; set; }
        public int IdState { get; set; } = 1;

        public virtual Card IdCardNavigation { get; set; }
        public virtual TransactionState IdStateNavigation { get; set; }
    }
}
