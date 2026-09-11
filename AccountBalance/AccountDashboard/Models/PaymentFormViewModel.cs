using System.ComponentModel.DataAnnotations;

namespace AccountDashboard.Web.Models
{
    /// <summary>
    /// Formulario "Registrar pago". Igual que en compras, el Id de tarjeta no viaja como
    /// campo visible; se toma de la tarjeta seleccionada en Session.
    /// </summary>
    public class PaymentFormViewModel
    {
        public string CardNumberMasked { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la fecha del pago.")]
        [Display(Name = "Fecha de pago")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; } = DateTime.Today;

        [StringLength(300, ErrorMessage = "La descripcion no puede exceder 300 caracteres.")]
        [Display(Name = "Descripcion (opcional)")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Ingrese el monto del pago.")]
        [Range(0.01, 999999.99, ErrorMessage = "El monto debe ser mayor a $0.00.")]
        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
