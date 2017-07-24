namespace System
{
    /// <summary>
    /// Contains extension methods for <see cref="Type" />.
    /// </summary>
    internal static class TypeExtensions
    {
        public static bool Implements<I>(this Type type)
            where I : class
        {
            var i = typeof(I);
            if (!i.IsInterface)
                throw new ArgumentException("Only interfaces can be 'implemented'.");

            return i.IsAssignableFrom(type);
        }
    }
}
