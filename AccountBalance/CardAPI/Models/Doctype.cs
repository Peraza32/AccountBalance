using System;
using System.Collections.Generic;

namespace CardAPI.Models
{
    public partial class Doctype
    {
        public Doctype()
        {
            Clients = new HashSet<Client>();
        }

        public int Id { get; set; }
        public string Docname { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
