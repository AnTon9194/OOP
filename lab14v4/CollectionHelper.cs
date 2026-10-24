using System;
using System.Collections.Generic;

namespace lab14v4
{
    public static class CollectionHelper<T>
    {
        public static void ForEach(IEnumerable<T> collection, Action<T> action)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var item in collection)
            {
                action(item);
            }
        }
    }
}