namespace Lab9SelfV1;

public static class CollectionUtils
{
    /// <summary>Перетворює кожен елемент колекції (аналог LINQ Select).</summary>
    public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(mapper);

        var result = new List<TResult>();
        foreach (var item in collection)
            result.Add(mapper(item));
        return result;
    }

    /// <summary>Елемент з максимальним значенням ключа (аналог LINQ MaxBy).</summary>
    public static T MaxBy<T, TKey>(
        IEnumerable<T> collection,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(keySelector);

        T maxItem = default!;
        TKey maxKey = default!;
        bool first = true;
        foreach (var item in collection)
        {
            TKey key = keySelector(item);
            if (first || key.CompareTo(maxKey) > 0)
            {
                maxKey = key;
                maxItem = item;
                first = false;
            }
        }

        if (first)
            throw new InvalidOperationException("Колекція порожня.");
        return maxItem;
    }

    /// <summary>Елементи, що задовольняють умову (аналог LINQ Where).</summary>
    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection,
        Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        var result = new List<T>();
        foreach (var item in collection)
        {
            if (predicate(item))
                result.Add(item);
        }
        return result;
    }

    /// <summary>Виконує дію для кожного елемента.</summary>
    public static void ForEach<T>(
        IEnumerable<T> collection,
        Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in collection)
            action(item);
    }
}
