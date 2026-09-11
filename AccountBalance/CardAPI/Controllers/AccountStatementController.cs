using CardAPI.Application.Client.Queries.AccountBalance;
using CardAPI.Application.Client.Queries.ClientData;
using CardAPI.Reports;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace CardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountStatementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountStatementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}/pdf")]
        public async Task<IActionResult> ExportPdf(string userId)
        {
            var userData = await _mediator.Send(new ClientDataByName(userId));
            var balance = await _mediator.Send(new GetAccountBalance(userData.idCard, userData.clientId));

            var document = new AccountStatementDocument(balance);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"EstadoCuenta_{userId}.pdf");
        }
    }
}
