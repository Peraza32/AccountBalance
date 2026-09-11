using System.ComponentModel.DataAnnotations;

namespace AccountDashboard.Web.Models
{
    
    public class CreateClientViewModel
    {
        [Required(ErrorMessage = "Ingrese el numero de documento.")]
        [StringLength(20)]
        [Display(Name = "Numero de documento (DOC_NUMBER)")]
        public string DocNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el nombre completo.")]
        [StringLength(250)]
        [Display(Name = "Nombre completo")]
        public string ClientName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Ingrese un numero de telefono valido.")]
        [Display(Name = "Telefono")]
        public string? Cellphone { get; set; }

        [EmailAddress(ErrorMessage = "Ingrese un correo valido.")]
        [Display(Name = "Correo electronico")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Seleccione el tipo de documento.")]
        [Display(Name = "Tipo de documento")]
        public string DocType { get; set; } = "DUI";
    }
}
