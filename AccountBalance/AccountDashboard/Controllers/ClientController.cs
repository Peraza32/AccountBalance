using AccountDashboard.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountDashboard.Web.Controllers
{
    /// <summary>
    /// PLACEHOLDER: no existe endpoint de creacion de clientes en ClientController/CardController
    /// (backend adjuntado). Esta pantalla deja lista la captura de datos y la validacion de
    /// presentacion; el envio real queda deshabilitado y claramente identificado como pendiente.
    /// Cuando el backend exponga el endpoint (por ejemplo POST api/Client), agregar el metodo
    /// correspondiente a IClientApiService y reemplazar el cuerpo de Create(POST).
    /// </summary>
    public class ClientController : Controller
    {
        public IActionResult Create()
        {
            return View(new CreateClientViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateClientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // No se envia a ningun lado: el backend no expone un endpoint de creacion de clientes.
            ModelState.AddModelError(string.Empty,
                "Esta funcionalidad es un placeholder: el backend aun no expone un endpoint para crear clientes. " +
                "El formulario y su validacion ya estan listos para conectarse en cuanto el endpoint exista.");
            return View(model);
        }
    }
}
