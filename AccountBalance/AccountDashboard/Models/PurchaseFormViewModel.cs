using System.ComponentModel.DataAnnotations;

namespace AccountDashboard.Web.Models
{
    /// <summary>
    /// Formulario "Registrar compra". No incluye el Id de la tarjeta como campo visible:
    /// el controlador lo toma de la tarjeta seleccionada en Session (dato sensible).
    /// </summary>
    public class PurchaseFormViewModel
    {
        public string CardNumberMasked { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la fecha de la compra.")]
        [Display(Name = "Fecha de compra")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Ingrese una descripcion.")]
        [StringLength(200, ErrorMessage = "La descripcion no puede exceder 200 caracteres.")]
        [Display(Name = "Descripcion")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el monto de la compra.")]
        [Range(0.01, 999999.99, ErrorMessage = "El monto debe ser mayor a $0.00.")]
        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
