using System.Collections.Concurrent;
using System.Diagnostics;

Console.WriteLine($"Логічних ядер: {Environment.ProcessorCount}\n");

// ============ Частина 1: LINQ vs PLINQ ============
Console.WriteLine("=== LINQ vs PLINQ (перевірка на просте число + корінь) ===");
foreach (int size in new[] { 1_000_000, 10_000_000 })
    Benchmark(size);

// ============ Частина 2: побічні ефекти ============
Console.WriteLine("\n=== Побічні ефекти у PLINQ ===");
SideEffectsDemo();

// -----------------------------------------------------------------

static List<int> GenerateData(int size)
{
    var rnd = new Random(42);                       // фіксований Seed
    var list = new List<int>(size);
    for (int i = 0; i < size; i++)
        list.Add(rnd.Next(2, 200_000));
    return list;
}

// Обчислювально "дорога" операція: перевірка на просте число пробним діленням
static bool IsPrime(int n)
{
    if (n < 2) return false;
    if (n % 2 == 0) return n == 2;
    for (int i = 3; (long)i * i <= n; i += 2)
        if (n % i == 0) return false;
    return true;
}

static void Benchmark(int size)
{
    var data = GenerateData(size);

    var sw = Stopwatch.StartNew();
    var linq = data.Where(IsPrime).Select(x => Math.Sqrt(x)).ToList();
    sw.Stop();
    double linqMs = sw.Elapsed.TotalMilliseconds;

    sw.Restart();
    var plinq = data.AsParallel().Where(IsPrime).Select(x => Math.Sqrt(x)).ToList();
    sw.Stop();
    double plinqMs = sw.Elapsed.TotalMilliseconds;

    Console.WriteLine($"\nРозмір: {size:N0}");
    Console.WriteLine($"  LINQ : {linqMs,9:F1} мс  (знайдено {linq.Count:N0})");
    Console.WriteLine($"  PLINQ: {plinqMs,9:F1} мс  (знайдено {plinq.Count:N0})");
    Console.WriteLine($"  Прискорення: x{linqMs / plinqMs:F2}");
    Console.WriteLine($"  Кількість збігається: {linq.Count == plinq.Count}");
}

static void SideEffectsDemo()
{
    var data = GenerateData(1_000_000);
    int expected = data.Count(IsPrime);   // еталон: послідовний LINQ
    Console.WriteLine($"Еталонна кількість простих: {expected:N0}\n");

    // --- ПРОБЛЕМА: спільна змінна без синхронізації ---
    Console.WriteLine("1) Без синхронізації (counter++ у ForAll):");
    for (int run = 1; run <= 3; run++)
    {
        int counter = 0;
        data.AsParallel().Where(IsPrime).ForAll(_ => counter++);   // гонка даних!
        Console.WriteLine($"   запуск {run}: counter = {counter:N0}  (втрачено {expected - counter:N0})");
    }

    // --- ПРОБЛЕМА 2: небезпечна колекція ---
    Console.WriteLine("\n2) List<int>.Add з кількох потоків:");
    try
    {
        var unsafeList = new List<int>();
        data.AsParallel().Where(IsPrime).ForAll(x => unsafeList.Add(x));
        Console.WriteLine($"   Count = {unsafeList.Count:N0} (очікувалось {expected:N0})");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"   Виняток: {ex.GetType().Name} - {ex.InnerException?.Message ?? ex.Message}");
    }

    // --- ВИПРАВЛЕННЯ ---
    Console.WriteLine("\n3) Виправлення:");

    int lockCounter = 0;
    object sync = new();
    var sw = Stopwatch.StartNew();
    data.AsParallel().Where(IsPrime).ForAll(_ => { lock (sync) { lockCounter++; } });
    Console.WriteLine($"   lock:               {lockCounter:N0}  ({sw.Elapsed.TotalMilliseconds:F1} мс)");

    int interlocked = 0;
    sw.Restart();
    data.AsParallel().Where(IsPrime).ForAll(_ => Interlocked.Increment(ref interlocked));
    Console.WriteLine($"   Interlocked:        {interlocked:N0}  ({sw.Elapsed.TotalMilliseconds:F1} мс)");

    var bag = new ConcurrentBag<int>();
    sw.Restart();
    data.AsParallel().Where(IsPrime).ForAll(x => bag.Add(x));
    Console.WriteLine($"   ConcurrentBag:      {bag.Count:N0}  ({sw.Elapsed.TotalMilliseconds:F1} мс)");

    sw.Restart();
    int pure = data.AsParallel().Count(IsPrime);   // найкраще: без спільного стану
    Console.WriteLine($"   Без побічних ефектів (Count): {pure:N0}  ({sw.Elapsed.TotalMilliseconds:F1} мс)");
}
