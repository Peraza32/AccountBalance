using CardAPI.Domain.Entities.DTO;
using CardAPI.Utils.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        // Post: api/<CardController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CardController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CardController>
        [HttpPost]
        public async Task<IActionResult> Purchase([FromBody] PurchaseDTO purchase)
        {
            try { 
                PurchaseValidator purchaseValidator = new PurchaseValidator();
                ValidationResult validationResult = purchaseValidator.Validate(purchase);

                if(!validationResult.IsValid)
                {
                    return BadRequest("Invalid purchase data");   
                }

                return Ok();

            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the purchase.");
            }
        }

        // PUT api/<CardController>/5
        [HttpPost]
        public async Task<IActionResult> Payment([FromBody] CardPaymentDTO payment)
        {
            try
            {
                PaymentValidator paymentValidator = new PaymentValidator();
                ValidationResult validationResult = paymentValidator.Validate(payment);

                if (!validationResult.IsValid) { 
                
                    return BadRequest("Invalid payment data");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the payment.");
            }
        }

        // DELETE api/<CardController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
