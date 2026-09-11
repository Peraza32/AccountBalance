using AutoMapper;
using AccountDashboard.Web.ApiContracts;
using AccountDashboard.Web.Common;
using AccountDashboard.Web.Models;
using AccountDashboard.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountDashboard.Web.Controllers
{
    /
    public class PaymentController : Controller
    {
        private readonly ICardApiService _cardApiService;
        private readonly IMapper _mapper;

        public PaymentController(ICardApiService cardApiService, IMapper mapper)
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

            return View(new PaymentFormViewModel { CardNumberMasked = selectedCard.CardNumberMasked });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentFormViewModel model)
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

            var request = _mapper.Map<PaymentRequestDto>(model);
            request.cardId = selectedCard.CardId; // se asigna desde Session, nunca desde el formulario

            var result = await _cardApiService.RegisterPaymentAsync(request);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No fue posible registrar el pago.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Pago registrado correctamente.";
            return RedirectToAction("Index", "AccountStatement");
        }
    }
}
