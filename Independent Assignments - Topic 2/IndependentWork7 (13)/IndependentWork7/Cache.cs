namespace IndependentWork7;

/// <summary>
/// Узагальнений кеш з обмеженою ємністю та політикою FIFO.
/// </summary>
public class Cache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, TValue> _cache;
    private readonly Queue<TKey> _keysOrder;

    public Cache(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Ємність має бути більшою за нуль.");

        _capacity = capacity;
        _cache = new Dictionary<TKey, TValue>(capacity);
        _keysOrder = new Queue<TKey>(capacity);
    }

    public int Count => _cache.Count;

    public void Add(TKey key, TValue value)
    {
        // Ключ уже є: оновлюємо значення, порядок FIFO не змінюється.
        if (_cache.ContainsKey(key))
        {
            _cache[key] = value;
            return;
        }

        // Кеш повний: видаляємо найстаріший елемент.
        if (_cache.Count >= _capacity)
        {
            TKey oldest = _keysOrder.Dequeue();
            _cache.Remove(oldest);
        }

        _cache[key] = value;
        _keysOrder.Enqueue(key);
    }

    public TValue Get(TKey key)
    {
        if (_cache.TryGetValue(key, out TValue? value))
            return value;

        throw new KeyNotFoundException($"Ключ '{key}' не знайдено в кеші.");
    }

    public bool ContainsKey(TKey key) => _cache.ContainsKey(key);
}
