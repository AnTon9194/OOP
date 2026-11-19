namespace lab18vN;

/// <summary>
/// Допоміжний клас для повторного виконання операцій, що можуть тимчасово завершуватись помилкою.
/// </summary>
public static class RetryHelper
{
    private static readonly TimeSpan DefaultDelay = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// Виконує операцію з повторними спробами та експоненційною затримкою.
    /// </summary>
    /// <param name="operation">Операція, яку потрібно виконати.</param>
    /// <param name="retryCount">Максимальна кількість ПОВТОРНИХ спроб (загалом спроб = retryCount + 1).</param>
    /// <param name="initialDelay">Початкова затримка; перед повтором №k затримка = initialDelay * 2^k.</param>
    /// <param name="shouldRetry">Визначає, чи слід повторювати спробу для конкретного винятку (null — повторювати для будь-якого).</param>
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentOutOfRangeException.ThrowIfNegative(retryCount);

        if (initialDelay == default)
            initialDelay = DefaultDelay;

        shouldRetry ??= _ => true;

        for (int attempt = 0; ; attempt++)
        {
            try
            {
                Log($"Спроба {attempt + 1} з {retryCount + 1}...");
                T result = operation();
                Log($"Спроба {attempt + 1}: УСПІХ.");
                return result;
            }
            catch (Exception ex)
            {
                Log($"Спроба {attempt + 1}: НЕВДАЧА — {ex.GetType().Name}: {ex.Message}");

                if (!shouldRetry(ex))
                {
                    Log("Виняток не підлягає повтору (shouldRetry = false). Перекидаємо далі.");
                    throw;
                }

                if (attempt >= retryCount)
                {
                    Log("Вичерпано всі повторні спроби. Перекидаємо виняток далі.");
                    throw;
                }

                // Експоненційна затримка: initialDelay * 2^attempt
                TimeSpan delay = TimeSpan.FromTicks(initialDelay.Ticks * (1L << attempt));
                Log($"Очікування {delay.TotalMilliseconds:F0} мс перед наступною спробою.");
                Thread.Sleep(delay);
            }
        }
    }

    private static void Log(string message) =>
        Console.WriteLine($"    [{DateTime.Now:HH:mm:ss.fff}] [Retry] {message}");
}
