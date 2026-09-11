using System.Diagnostics;
using AutoMapper;
using AccountDashboard.Web.ApiContracts;
using AccountDashboard.Web.Common;
using AccountDashboard.Web.Models;
using AccountDashboard.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CardAPI.Web.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IClientApiService _clientApiService;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IClientApiService clientApiService, IMapper mapper, ILogger<HomeController> logger)
        {
            _clientApiService = clientApiService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ClientSearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ClientSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _clientApiService.GetClientCardsAsync(model.UserId);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No fue posible consultar al cliente.");
                return View(model);
            }

            var cards = result.Data ?? new ClientWithCardDto();

            if (cards.cardNumber == string.Empty)
            {
                ModelState.AddModelError(string.Empty, "No se encontraron tarjetas asociadas a ese numero de documento.");
                return View(model);
            }

            if (cards  != null)
            {
                SelectCardAndRedirect(cards);
                return RedirectToAction("Index", "AccountStatement");
            }

            var selectViewModel = new SelectCardViewModel
            {
                UserId = model.UserId,
                ClientName = cards.clientName,
                Cards = _mapper.Map<List<ClientCardOptionViewModel>>(cards)
            };

            
            TempData.Remove("ClientCardsJson");
            TempData["ClientCardsJson"] = System.Text.Json.JsonSerializer.Serialize(cards);
            TempData.Keep("ClientCardsJson");

            return View("SelectCard", selectViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectCard(SelectCardViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SelectedCardId) ||
                TempData.Peek("ClientCardsJson") is not string cardsJson)
            {
                ModelState.AddModelError(string.Empty, "La sesion de busqueda expiro. Intente nuevamente.");
                return RedirectToAction(nameof(Index));
            }

            TempData.Keep("ClientCardsJson");

            var cards = System.Text.Json.JsonSerializer.Deserialize<List<ClientWithCardDto>>(cardsJson) ?? new();
            var chosen = cards.FirstOrDefault(c => c.idCard.ToString() == model.SelectedCardId);

            if (chosen is null)
            {
                ModelState.AddModelError(string.Empty, "La tarjeta seleccionada no es valida.");
                model.Cards = _mapper.Map<List<ClientCardOptionViewModel>>(cards);
                return View(model);
            }

            SelectCardAndRedirect(chosen);
            return RedirectToAction("Index", "AccountStatement");
        }

       
        public IActionResult ChangeClient()
        {
            HttpContext.Session.ClearSelectedCard();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void SelectCardAndRedirect(ClientWithCardDto card)
        {
            HttpContext.Session.SetSelectedCard(new SelectedCardContext
            {
                CardId = card.idCard,
                UserId = card.clientId,
                ClientName = card.clientName,
                CardNumberMasked = CardMasking.FormatCardNumber(card.cardNumber)
            });

            _logger.LogInformation(
                "Cliente {UserId} selecciono tarjeta {CardMasked}",
                card.clientId, CardMasking.ForLog(card.idCard));
        }
    }
}
