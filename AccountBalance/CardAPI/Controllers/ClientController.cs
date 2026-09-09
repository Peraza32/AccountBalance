using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        //GET api/<CardController>/Balance
        [HttpGet("Balance")]
        public async Task<IActionResult> GetBalance()
        {
            return Ok();
        }


    }
}
