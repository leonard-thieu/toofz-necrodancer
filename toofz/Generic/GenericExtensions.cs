using System;
using System.Collections.Generic;

namespace toofz.Generic
{
    public static class GenericExtensions
    {
        /// <summary>
        /// Flattens a hierarchical structure.
        /// </summary>
        /// <typeparam name="TSource">The type of <paramref name="source" />.</typeparam>
        /// <param name="source">The hierarchical item to flatten.</param>
        /// <param name="nextItem">A function that determines the next item.</param>
        /// <returns>The flattened sequence of the hierarchical structure.</returns>
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem)
            where TSource : class
        {

            return FromHierarchy(source, nextItem, s => s != null);
        }

        /// <summary>
        /// Flattens a hierarchical structure.
        /// </summary>
        /// <typeparam name="TSource">The type of <paramref name="source" />.</typeparam>
        /// <param name="source">The hierarchical item to flatten.</param>
        /// <param name="nextItem">A function that determines the next item.</param>
        /// <param name="canContinue">A function that determines when to stop iteration.</param>
        /// <returns>The flattened sequence of the hierarchical structure.</returns>
        private static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
                yield return current;
        }
    }
}
