using System;
using System.Collections.Generic;

namespace CardAPI.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public string LogDescription { get; set; }
        public DateTime? LogDt { get; set; }
    }
}
