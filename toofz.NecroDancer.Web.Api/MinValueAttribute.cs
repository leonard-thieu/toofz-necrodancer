using System;
using System.ComponentModel.DataAnnotations;

namespace toofz.NecroDancer.Web.Api
{
    internal sealed class MinValueAttribute : ValidationAttribute
    {
        public MinValueAttribute(int minimum)
        {
            Minimum = minimum;
        }

        public int Minimum { get; }

        public override bool IsValid(object value)
        {
            var other = Convert.ToInt32(value);

            return other >= Minimum;
        }
    }
}