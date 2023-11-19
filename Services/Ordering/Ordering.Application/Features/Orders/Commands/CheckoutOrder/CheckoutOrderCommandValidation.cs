
using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidation : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidation()
        {
            RuleFor(o => o.UserName)
                .NotEmpty().WithMessage("{UserName} is required")
                .NotNull().WithMessage("{UserName} is required")
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters");

            RuleFor(o => o.EmailAddress)
                .NotEmpty().WithMessage("{Email} is required");

            RuleFor(o => o.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required")
                .GreaterThanOrEqualTo(0).WithMessage("{TotalPrice} should be greator than 0");
        }
    }
}
