using CardAPI.Domain.Entities.DTO;
using FluentValidation;

namespace CardAPI.Utils.Validators
{
    public class PaymentValidator:AbstractValidator<CardPaymentDTO>
    {
        public PaymentValidator() { 
        
            RuleFor(payment => payment.cardId).NotEqual(Guid.Empty).WithMessage("Card ID is required.");
            RuleFor(payment => payment.paymentDate).NotEmpty().WithMessage("Payment date is required.");
            RuleFor(payment => payment.amount).GreaterThan(0).WithMessage("Payment amount must be greater than zero.");
            RuleFor(payment => payment.description).NotEmpty().WithMessage("Payment description is required.");
        }
    }
}
