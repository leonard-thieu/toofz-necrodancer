using System.Linq;

namespace toofz
{
    public static class EnumerationExtensions
    {
        public static bool IsAny<T>(this T enumeration, params T[] items)
            where T : Enumeration
        {
            if (enumeration == null)
                return false;
            return items.Any(t => enumeration.Equals(t));
        }
    }
}
