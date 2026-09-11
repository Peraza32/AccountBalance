using CardAPI.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CardAPI.Domain.Models
{
    public partial class Client
    {
        public Client()
        {
            Cards = new HashSet<Card>();
        }

        public string DocNumber { get; set; }
        public string ClientName { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreationUser { get; set; }
        public int IdDoctype { get; set; }

        public virtual Doctype IdDoctypeNavigation { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
    }
}
