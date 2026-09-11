namespace AccountDashboard.Web.Models
{
    public class TransactionHistoryViewModel
    {
        public string CardNumberMasked { get; set; } = string.Empty;
        public List<TransactionItemViewModel> Transactions { get; set; } = new();
    }

    public class TransactionItemViewModel
    {
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public int State { get; set; }

      
        public string StateLabel => State switch
        {
            1 => "Pendiente",
            2 => "En proceso",
            3 => "Finalizada",
            4 => "Fallida",
            _ => "Desconocido"
        };
    }
}
