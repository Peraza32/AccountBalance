using AutoMapper;
using AccountDashboard.Web.ApiContracts;
using AccountDashboard.Web.Common;
using AccountDashboard.Web.Models;
using AccountDashboard.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountDashboard.Web.Controllers
{
   
    public class TransactionHistoryController : Controller
    {
        private readonly IClientApiService _clientApiService;
        private readonly IMapper _mapper;

        public TransactionHistoryController(IClientApiService clientApiService, IMapper mapper)
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

            var request = new TransactionHistoryRequestDto
            {
                CardId = selectedCard.CardId,
                UserId = selectedCard.UserId
            };

            var result = await _clientApiService.GetTransactionHistoryAsync(request);

            var viewModel = new TransactionHistoryViewModel
            {
                CardNumberMasked = selectedCard.CardNumberMasked
            };

            if (!result.Success || result.Data is null)
            {
                ViewBag.ErrorMessage = result.ErrorMessage ?? "No fue posible obtener el historial de transacciones.";
                return View(viewModel);
            }

            viewModel.Transactions = _mapper.Map<List<TransactionItemViewModel>>(result.Data)
            .OrderByDescending(t => t.Date)
            .ToList();

            return View(viewModel);
        }
    }
}
