using CardAPI.Application.Client.Queries.AccountBalance;
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

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //GET api/<CardController>/Balance
        [HttpGet("Balance")]
        public async Task<IActionResult> GetBalance([FromBody] GetAccountBalance query)
        {
            var result = await _mediator.Send(query);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


    }
}
