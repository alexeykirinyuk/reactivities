using FluentValidation;

namespace Application.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> rules)
        {
            return rules
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches("[A-Z]").WithMessage("Password must contains at least 1 uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contains at least 1 lowercase letter.")
                .Matches("[1-9]").WithMessage("Password must contains a number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contains non alphanumeric.");
        }
    }
}