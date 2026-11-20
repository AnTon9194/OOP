using IndependentWork19;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;

// ============ Демонстрація власного винятку ============
Console.WriteLine("=== Власний виняток ===");
var account = new BankAccount(100m);
try
{
    account.Withdraw(250m);
}
catch (InsufficientFundsException ex)
{
    Console.WriteLine($"Помилка: {ex.Message}");
    Console.WriteLine($"Бракує: {ex.RequestedAmount - ex.CurrentBalance:F2} грн");
}
finally
{
    Console.WriteLine("Операцію завершено.\n");
}

await Scenario1_RetryApi();
await Scenario2_CircuitBreakerDb();
await Scenario3_TimeoutFallback();

// ============ Сценарій 1: Retry + exponential backoff + jitter ============
// Зовнішнє API погоди тимчасово недоступне (перші 2 виклики дають збій).
static async Task Scenario1_RetryApi()
{
    Console.WriteLine("=== Сценарій 1: Retry (зовнішнє API) ===");
    int calls = 0;

    var pipeline = new ResiliencePipelineBuilder()
    .AddRetry(new RetryStrategyOptions
    {
        ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
              MaxRetryAttempts = 3,
              Delay = TimeSpan.FromMilliseconds(500),
              BackoffType = DelayBackoffType.Exponential,
              UseJitter = true,
              OnRetry = args =>
              {
                  Console.WriteLine($"  Retry #{args.AttemptNumber + 1} через " +
                  $"{args.RetryDelay.TotalSeconds:F1}с: {args.Outcome.Exception?.Message}");
                  return default;
              }
    })
    .Build();

    try
    {
        string result = await pipeline.ExecuteAsync(async ct =>
        {
            calls++;
            Console.WriteLine($"  Виклик API, спроба {calls}...");
            await Task.Delay(50, ct);

            if (calls < 3)
                throw new HttpRequestException("503 Service Unavailable");

            return "+18°C, хмарно";
        });

        Console.WriteLine($"  Успіх: {result}\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  Остаточна невдача: {ex.Message}\n");
    }
}

// ============ Сценарій 2: Circuit Breaker ============
// База даних «лежить»: після кількох збоїв припиняємо її навантажувати.
static async Task Scenario2_CircuitBreakerDb()
{
    Console.WriteLine("=== Сценарій 2: Circuit Breaker (база даних) ===");
    bool dbIsUp = false;

    var pipeline = new ResiliencePipelineBuilder()
    .AddCircuitBreaker(new CircuitBreakerStrategyOptions
    {
        ShouldHandle = new PredicateBuilder().Handle<InvalidOperationException>(),
                       FailureRatio = 0.5,
                       MinimumThroughput = 3,
                       SamplingDuration = TimeSpan.FromSeconds(10),
                       BreakDuration = TimeSpan.FromSeconds(2),
                       OnOpened = args =>
                       {
                           Console.WriteLine($"  >>> Вимикач РОЗІМКНЕНО на {args.BreakDuration.TotalSeconds}с");
                           return default;
                       },
                       OnHalfOpened = _ =>
                       {
                           Console.WriteLine("  >>> Вимикач НАПІВРОЗІМКНЕНО (пробний запит)");
                           return default;
                       },
                       OnClosed = _ =>
                       {
                           Console.WriteLine("  >>> Вимикач ЗАМКНЕНО (сервіс відновився)");
                           return default;
                       }
    })
    .Build();

    async Task CallDb(int n)
    {
        try
        {
            await pipeline.ExecuteAsync(async ct =>
            {
                Console.WriteLine($"  Запит #{n} до БД...");
                await Task.Delay(20, ct);
                if (!dbIsUp)
                    throw new InvalidOperationException("Не вдалося підключитися до БД");
                Console.WriteLine($"  Запит #{n} успішний");
            });
        }
        catch (BrokenCircuitException)
        {
            Console.WriteLine($"  Запит #{n} відхилено миттєво (коло розімкнене)");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Запит #{n} невдалий: {ex.Message}");
        }
    }

    for (int i = 1; i <= 6; i++)
        await CallDb(i);

    Console.WriteLine("  ...чекаємо 2.5с, БД відновлюється...");
    dbIsUp = true;
    await Task.Delay(TimeSpan.FromSeconds(2.5));

    await CallDb(7);
    await CallDb(8);
    Console.WriteLine();
}

// ============ Сценарій 3: Timeout + Fallback ============
// Сервіс курсів валют відповідає надто повільно: обмежуємо час і віддаємо кешоване значення.
static async Task Scenario3_TimeoutFallback()
{
    Console.WriteLine("=== Сценарій 3: Timeout + Fallback (курс валют) ===");

    var pipeline = new ResiliencePipelineBuilder<string>()
    .AddFallback(new FallbackStrategyOptions<string>
    {
        ShouldHandle = new PredicateBuilder<string>().Handle<TimeoutRejectedException>(),
                 FallbackAction = _ => Outcome.FromResultAsValueTask("41.50 (кешоване значення)"),
                 OnFallback = _ =>
                 {
                     Console.WriteLine("  Fallback: використовуємо кешований курс");
                     return default;
                 }
    })
    .AddTimeout(TimeSpan.FromSeconds(1))
    .Build();

    string rate = await pipeline.ExecuteAsync(async ct =>
    {
        Console.WriteLine("  Запит курсу USD/UAH (сервіс відповідає 3с)...");
        await Task.Delay(TimeSpan.FromSeconds(3), ct);   // ct скасується за таймаутом
        return "41.62";
    });

    Console.WriteLine($"  Курс: {rate}\n");
}
