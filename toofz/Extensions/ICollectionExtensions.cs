namespace System.Collections.Generic
{
    public static class ICollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
