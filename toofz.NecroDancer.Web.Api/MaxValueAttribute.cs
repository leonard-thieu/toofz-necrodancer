using System;
using System.ComponentModel.DataAnnotations;

namespace toofz.NecroDancer.Web.Api
{
    internal sealed class MaxValueAttribute : ValidationAttribute
    {
        public MaxValueAttribute(int maximum)
        {
            Maximum = maximum;
        }

        public int Maximum { get; }

        public override bool IsValid(object value)
        {
            var other = Convert.ToInt32(value);

            return other <= Maximum;
        }
    }
}