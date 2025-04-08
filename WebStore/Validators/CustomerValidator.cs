using FluentValidation;
using WebStore.Models;

namespace WebStore.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Email).NotEmpty().EmailAddress();
        }
    }
}
