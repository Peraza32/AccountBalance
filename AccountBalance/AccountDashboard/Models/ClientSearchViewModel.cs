using System.ComponentModel.DataAnnotations;

namespace AccountDashboard.Web.Models
{
    /// <summary>
    /// Formulario de busqueda de cliente. El endpoint real de autenticacion/seleccion de
    /// cliente no fue provisto en esta prueba (no hay un "listado de clientes" en los
    /// controladores adjuntos); se usa el identificador de documento (DOC_NUMBER en la base
    /// de datos) para consultar GET api/Client/{userId}, unico endpoint disponible para
    /// obtener las tarjetas de un cliente.
    /// </summary>
    public class ClientSearchViewModel
    {
        [Required(ErrorMessage = "Ingrese el numero de documento del cliente.")]
        [StringLength(20, ErrorMessage = "El numero de documento no puede exceder 20 caracteres.")]
        [Display(Name = "Numero de documento (DUI/NIT)")]
        public string UserId { get; set; } = string.Empty;
    }
}
