using FluentValidation;

namespace toofz.NecroDancer.Dungeons
{
    static class IRuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, int> IsIntegralBoolean<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new IntegralBooleanValidator());
        }
    }
}
