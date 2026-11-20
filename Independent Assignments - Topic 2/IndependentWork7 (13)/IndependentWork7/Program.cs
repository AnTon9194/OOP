using IndependentWork7;

var cache = new Cache<string, string>(3);

Console.WriteLine("=== Додаємо 3 елементи (ємність = 3) ===");
cache.Add("UA", "Україна");
cache.Add("PL", "Польща");
cache.Add("DE", "Німеччина");
Console.WriteLine($"Count = {cache.Count}");

Console.WriteLine($"Get(\"PL\") = {cache.Get("PL")}");

Console.WriteLine("\n=== Додаємо 4-й елемент: кеш переповнюється ===");
cache.Add("FR", "Франція");

foreach (string key in new[] { "UA", "PL", "DE", "FR" })
    Console.WriteLine($"ContainsKey(\"{key}\") = {cache.ContainsKey(key)}");

Console.WriteLine($"Count = {cache.Count}");

Console.WriteLine("\n=== Звернення до видаленого ключа ===");
try
{
    cache.Get("UA");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"Виняток: {ex.Message}");
}
