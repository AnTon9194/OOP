namespace IndependentWork14;

/// <summary>
/// Політика повторних спроб з експоненційною затримкою та випадковим jitter.
/// </summary>
public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _jitterFactor;
    private readonly Random _random = new();

    public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
    {
        if (maxRetries < 0)
            throw new ArgumentOutOfRangeException(nameof(maxRetries));
        if (initialDelay < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(initialDelay));
        if (jitterFactor is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(jitterFactor), "Очікується значення від 0 до 1.");

        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
    }

    /// <param name="operation">Операція, яку потрібно виконати.</param>
    /// <param name="shouldRetry">Чи повторювати спробу для даного винятку (null = для будь-якого).</param>
    public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        for (int attempt = 0; ; attempt++)
        {
            try
            {
                Console.WriteLine($"  [Спроба {attempt + 1}/{_maxRetries + 1}] виконання...");
                T result = operation();
                Console.WriteLine($"  [Спроба {attempt + 1}] успіх.");
                return result;
            }
            catch (Exception ex)
            {
                bool retriesLeft = attempt < _maxRetries;
                bool retryAllowed = shouldRetry?.Invoke(ex) ?? true;

                if (!retriesLeft || !retryAllowed)
                {
                    string reason = !retryAllowed
                        ? "виняток не підлягає повтору"
                        : "вичерпано всі спроби";
                    Console.WriteLine($"  [Спроба {attempt + 1}] невдача: {ex.Message} -> {reason}.");
                    throw; // зберігаємо оригінальний стек
                }

                TimeSpan delay = CalculateDelay(attempt);
                Console.WriteLine($"  [Спроба {attempt + 1}] невдача: {ex.Message}. " +
                                  $"Очікування {delay.TotalMilliseconds:F0} мс перед повтором...");
                Thread.Sleep(delay);
            }
        }
    }

    // delay = initialDelay * 2^attempt * (1 + random * jitterFactor * 2 - jitterFactor)
    private TimeSpan CalculateDelay(int retryAttempt)
    {
        double jitter = 1 + _random.NextDouble() * _jitterFactor * 2 - _jitterFactor;
        double ms = _initialDelay.TotalMilliseconds * Math.Pow(2, retryAttempt) * jitter;
        return TimeSpan.FromMilliseconds(ms);
    }
}
