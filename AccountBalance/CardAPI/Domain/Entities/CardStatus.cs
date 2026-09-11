using System;
using System.Collections.Generic;

namespace CardAPI.Domain.Entities
{
    public partial class CardStatus
    {
        public CardStatus()
        {
            Cards = new HashSet<Card>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}
