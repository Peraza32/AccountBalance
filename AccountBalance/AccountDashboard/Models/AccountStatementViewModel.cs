namespace AccountDashboard.Web.Models
{
    /// <summary>ViewModel de presentacion para la pantalla "Estado de Cuenta". No es un DTO de API.</summary>
    public class AccountStatementViewModel
    {
        
        public string CardHolderName { get; set; } = string.Empty;

        /// <summary>Numero de tarjeta ya truncado por el backend (solo ultimos 4 digitos).</summary>
        public string CardNumberMasked { get; set; } = string.Empty;

        public decimal CurrentCredit { get; set; }
        public decimal AvailableCredit { get; set; }
        public decimal CreditLimit { get; set; }

        public List<PurchaseLineViewModel> Purchases { get; set; } = new();

        public decimal TotalActualMonthPurchases { get; set; }
        public decimal PreviousMonthPurchases { get; set; }

        public decimal BonificationInterest { get; set; }
        public decimal MinimumPayment { get; set; }

        /// <summary>Monto total a pagar = Saldo Total (currentCredit), segun especificacion de la prueba.</summary>
        public decimal TotalToPay => CurrentCredit;

        /// <summary>Monto total de contado con intereses = Saldo Total + Interes Bonificable (ya calculado por el backend).</summary>
        public decimal TotalWithInterest { get; set; }
    }

    public class PurchaseLineViewModel
    {
        public DateTime PurchaseDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
