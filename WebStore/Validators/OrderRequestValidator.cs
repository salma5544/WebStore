using FluentValidation;
using WebStore.DTOs;

namespace WebStore.Validators
{
    public class OrderRequestValidator : AbstractValidator<OrderRequestDto>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.ProductIds).NotEmpty().WithMessage("Order must contain at least one product.");
        }
    }
}
