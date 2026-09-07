using System;
using System.Collections.Generic;

namespace CardAPI.Models
{
    public partial class TransactionState
    {
        public TransactionState()
        {
            MovementsTcs = new HashSet<MovementsTc>();
            PaymentsTcs = new HashSet<PaymentsTc>();
        }

        public int Id { get; set; }
        public string Tstate { get; set; }

        public virtual ICollection<MovementsTc> MovementsTcs { get; set; }
        public virtual ICollection<PaymentsTc> PaymentsTcs { get; set; }
    }
}
