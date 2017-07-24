using FluentValidation;

namespace toofz.NecroDancer.Dungeons
{
    internal sealed class IntegralBooleanValidator : AbstractValidator<int>
    {
        public IntegralBooleanValidator()
        {
            RuleFor(x => x).InclusiveBetween(0, 1);
        }
    }
}
