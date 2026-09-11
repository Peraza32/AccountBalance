using AutoMapper;
using CardAPI.Application.Client.Queries.AccountBalance;
using CardAPI.Application.Client.Queries.ClientData;
using CardAPI.Application.Client.TransactionHistory;
using CardAPI.Domain.Entities.DAO;
using CardAPI.Domain.Entities.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ClientController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        //GET api/<CardController>/Balance
        [HttpPost("Balance")]
        public async Task<IActionResult> GetBalance([FromBody] AccountBalanceRequest request)
        {
            var command = _mapper.Map<AccountBalanceRequest, GetAccountBalance>(request);   
            var result = await _mediator.Send(command);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetClientData(string userId)
        {

            var result = await _mediator.Send(new ClientData(userId));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("username/{userName}")]
        public async Task<IActionResult> GetClientDataByName(string userName)
        {

            var result = await _mediator.Send(new ClientDataByName(userName));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("TransactionHistory")]
        public async Task<IActionResult> GetTransactionHistory([FromBody] TransactionHistoryRequestDTO request)
        {
            
            var result = await _mediator.Send(new GetTransactionHistory(request.CardId, request.UserId));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
