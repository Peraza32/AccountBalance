using AutoMapper;
using AccountDashboard.Web.ApiContracts;
using AccountDashboard.Web.Common;
using AccountDashboard.Web.Models;
using AccountDashboard.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountDashboard.Web.Controllers
{
    /// <summary>Pantalla de Estado de Cuenta (POST api/Client/Balance).</summary>
    public class AccountStatementController : Controller
    {
        private readonly IClientApiService _clientApiService;
        private readonly IMapper _mapper;

        public AccountStatementController(IClientApiService clientApiService, IMapper mapper)
        {
            _clientApiService = clientApiService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var selectedCard = HttpContext.Session.GetSelectedCard();
            if (selectedCard is null)
            {
                TempData["InfoMessage"] = "Seleccione un cliente y una tarjeta para continuar.";
                return RedirectToAction("Index", "Home");
            }

            var request = new AccountBalanceRequestDto
            {
                cardId = selectedCard.CardId,
                userId = selectedCard.UserId
            };

            var result = await _clientApiService.GetAccountBalanceAsync(request);

            if (!result.Success || result.Data is null)
            {
                ViewBag.ErrorMessage = result.ErrorMessage ?? "No fue posible obtener el estado de cuenta.";
                return View(new AccountStatementViewModel
                {
                    CardHolderName = selectedCard.ClientName,
                    CardNumberMasked = selectedCard.CardNumberMasked
                });
            }

            var viewModel = _mapper.Map<AccountStatementViewModel>(result.Data);
            return View(viewModel);
        }

        
    }
}
