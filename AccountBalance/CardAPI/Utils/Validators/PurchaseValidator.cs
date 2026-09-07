using CardAPI.Domain.Entities.DTO;
using FluentValidation;

namespace CardAPI.Utils.Validators
{
    public class PurchaseValidator:AbstractValidator<PurchaseDTO>
    {
        PurchaseValidator() { 
        
            RuleFor(purchase => purchase.cardId).NotEqual(Guid.Empty).WithMessage("Card ID is required.");
            RuleFor(purchase => purchase.purchaseDate).NotEmpty().WithMessage("Purchase date is required.");
            RuleFor(purchase => purchase.description).NotEmpty().WithMessage("Purchase description is required.");
            RuleFor(purchase => purchase.price).GreaterThan(0).WithMessage("Purchase price must be greater than zero.");
        }
    }
}
