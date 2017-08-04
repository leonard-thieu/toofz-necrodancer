using System;

namespace toofz.NecroDancer.Leaderboards
{
    public static class TypeExtensions
    {
        public static string GetSimpleFullName(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), $"{nameof(type)} is null.");

            return type.Namespace + "." + type.Name;
        }
    }
}
