using IndependentWork14;

var policy = new RetryPolicy(maxRetries: 4, initialDelay: TimeSpan.FromMilliseconds(200), jitterFactor: 0.2);

// --- Сценарій 1: збій двічі, потім успіх ---
Console.WriteLine("=== Сценарій 1: успіх після кількох невдач ===");
int calls = 0;
string data = policy.Execute(() =>
{
    calls++;
    if (calls < 3)
        throw new IOException($"Тимчасова помилка мережі (виклик {calls})");
    return "Дані отримано";
});
Console.WriteLine($"Результат: {data}\n");

// --- Сценарій 2: завжди невдача -> виняток після всіх спроб ---
Console.WriteLine("=== Сценарій 2: остаточна невдача ===");
try
{
    policy.Execute<int>(() => throw new TimeoutException("Сервер не відповідає"));
}
catch (TimeoutException ex)
{
    Console.WriteLine($"Остаточний виняток: {ex.GetType().Name}: {ex.Message}\n");
}

// --- Сценарій 3: shouldRetry забороняє повтор для нетимчасових помилок ---
Console.WriteLine("=== Сценарій 3: shouldRetry (ArgumentException не повторюється) ===");
try
{
    policy.Execute<string>(
        () => throw new ArgumentException("Некоректний запит"),
        ex => ex is IOException or TimeoutException);   // повторюємо лише тимчасові збої
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Остаточний виняток: {ex.GetType().Name}: {ex.Message}");
}
