using AutoMapper;
using AccountDashboard.Web.ApiContracts;
using AccountDashboard.Web.Common;
using AccountDashboard.Web.Models;
using AccountDashboard.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CardAPI.Web.Controllers
{
   
    public class PurchaseController : Controller
    {
        private readonly ICardApiService _cardApiService;
        private readonly IMapper _mapper;

        public PurchaseController(ICardApiService cardApiService, IMapper mapper)
        {
            _cardApiService = cardApiService;
            _mapper = mapper;
        }

        public IActionResult Create()
        {
            var selectedCard = HttpContext.Session.GetSelectedCard();
            if (selectedCard is null)
            {
                TempData["InfoMessage"] = "Seleccione un cliente y una tarjeta para continuar.";
                return RedirectToAction("Index", "Home");
            }

            return View(new PurchaseFormViewModel { CardNumberMasked = selectedCard.CardNumberMasked });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseFormViewModel model)
        {
            var selectedCard = HttpContext.Session.GetSelectedCard();
            if (selectedCard is null)
            {
                TempData["InfoMessage"] = "Seleccione un cliente y una tarjeta para continuar.";
                return RedirectToAction("Index", "Home");
            }

            model.CardNumberMasked = selectedCard.CardNumberMasked;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = _mapper.Map<PurchaseRequestDto>(model);
            request.cardId = selectedCard.CardId; // se asigna desde Session, nunca desde el formulario

            var result = await _cardApiService.RegisterPurchaseAsync(request);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No fue posible registrar la compra.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Compra registrada correctamente.";
            return RedirectToAction("Index", "AccountStatement");
        }
    }
}
