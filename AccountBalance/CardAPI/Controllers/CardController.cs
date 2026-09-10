using AutoMapper;
using Azure.Core;
using CardAPI.Application.Card.Command.Payment;
using CardAPI.Application.Card.Command.Purchase;
using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;
using CardAPI.Utils.Validators;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {       
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CardController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        


        // POST api/<CardController>/purchase
        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase([FromBody] PurchaseDTO purchase)
        {
            try { 
                PurchaseValidator purchaseValidator = new PurchaseValidator();
                ValidationResult validationResult = purchaseValidator.Validate(purchase);

                if(!validationResult.IsValid)
                {
                    return BadRequest("Invalid purchase data");   
                }
                var command = _mapper.Map<PurchaseDTO, AddPurchaseCommand>(purchase);
                await _mediator.Send(command);
                return Ok();

            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the purchase.");
            }
        }

        


        // POST api/<CardController>/payment
        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody] CardPaymentDTO payment)
        {
            try
            {
                PaymentValidator paymentValidator = new PaymentValidator();
                ValidationResult validationResult = paymentValidator.Validate(payment);

                if (!validationResult.IsValid) { 
                
                    return BadRequest("Invalid payment data");
                }
                var command = _mapper.Map<CardPaymentDTO, addPaymentCommand>(payment);
                await _mediator.Send(command);
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
