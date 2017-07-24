using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace toofz
{
    /// <summary>
    /// Contains utility methods.
    /// </summary>
    public static class Util
    {
        public static bool TryGetAttributeProperty<TResult, TAttribute>(MemberInfo member, Func<TAttribute, TResult> property, out TResult result)
            where TAttribute : Attribute
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var attr =
                member.GetCustomAttributes(typeof(TAttribute), true)
                      .OfType<TAttribute>()
                      .SingleOrDefault();

            if (attr != null)
            {
                result = property(attr);
                return true;
            }
            else
            {
                result = default(TResult);
                return false;
            }
        }

        public static string Invariant(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        public static string GetEnvVar(string variable)
        {
            return Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine) ??
                throw new ArgumentNullException(null,
                    $"The environment variable '{variable}' must be set.");
        }
    }
}
