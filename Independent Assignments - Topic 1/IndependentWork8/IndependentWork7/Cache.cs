using System;
using System.Collections.Generic;

namespace IndependentWork7;

/// <summary>
/// Узагальнений кеш з політикою витіснення FIFO (First In, First Out).
/// </summary>
/// <typeparam name="TKey">Тип ключа.</typeparam>
/// <typeparam name="TValue">Тип значення.</typeparam>
public class Cache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, TValue> _cache;
    private readonly Queue<TKey> _keysOrder;

    /// <summary>
    /// Конструктор, що ініціалізує кеш заданою ємністю.
    /// </summary>
    /// <param name="capacity">Максимальна кількість елементів у кеші.</param>
    /// <exception cref="ArgumentOutOfRangeException">Викидається, якщо ємність менше або дорівнює нулю.</exception>
    public Cache(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Ємність кешу повинна бути більшою за 0.");
        }

        _capacity = capacity;
        _cache = new Dictionary<TKey, TValue>(capacity);
        _keysOrder = new Queue<TKey>(capacity);
    }

    /// <summary>
    /// Поточна кількість елементів у кеші.
    /// </summary>
    public int Count => _cache.Count;

    /// <summary>
    /// Додає елемент до кешу або оновлює існуючий.
    /// Якщо кеш повний, видаляється найстаріший елемент за політикою FIFO.
    /// </summary>
    /// <param name="key">Ключ елемента.</param>
    /// <param name="value">Значення елемента.</param>
    public void Add(TKey key, TValue value)
    {
        if (_cache.ContainsKey(key))
        {
            _cache[key] = value;
            return;
        }

        if (_cache.Count >= _capacity)
        {
            TKey oldestKey = _keysOrder.Dequeue();
            _cache.Remove(oldestKey);
        }

        _cache[key] = value;
        _keysOrder.Enqueue(key);
    }

    /// <summary>
    /// Отримує значення за ключем.
    /// </summary>
    /// <param name="key">Ключ елемента.</param>
    /// <returns>Значення, що відповідає ключу.</returns>
    /// <exception cref="KeyNotFoundException">Викидається, якщо ключ не знайдено в кеші.</exception>
    public TValue Get(TKey key)
    {
        if (_cache.TryGetValue(key, out TValue? value))
        {
            return value!;
        }

        throw new KeyNotFoundException($"Ключ '{key}' не знайдено в кеші.");
    }

    /// <summary>
    /// Перевіряє, чи містить кеш заданий ключ.
    /// </summary>
    /// <param name="key">Ключ для перевірки.</param>
    /// <returns>True, якщо ключ існує в кеші; інакше False.</returns>
    public bool ContainsKey(TKey key)
    {
        return _cache.ContainsKey(key);
    }
}
