using System;
using System.Collections.Generic;

namespace CardAPI.Domain.Entities
{
    public partial class Card
    {
        public Guid Id { get; set; }
        public string IdClient { get; set; }
        public string CardNumber { get; set; }
        public string LastdCard { get; set; }
        public int IdCardStatus { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal AvCredit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal? Interest { get; set; }
        public decimal? MinInterest { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreationUser { get; set; }

        public virtual CardStatus IdCardStatusNavigation { get; set; }
    }
}
