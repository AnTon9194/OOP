using lab18vN;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Початкова затримка
TimeSpan delay = TimeSpan.FromMilliseconds(200);

// Повторювати лише для FileNotFoundException та HttpRequestException
Func<Exception, bool> retryPolicy = ex => ex is FileNotFoundException or HttpRequestException;

// Сценарій 1: FileProcessor (3 невдачі -> успіх)
Console.WriteLine("=== Сценарій 1: ReadImage (3 невдачі, потім успіх) ===");
var fileProcessor = new FileProcessor(failuresBeforeSuccess: 3);
try
{
    byte[] image = RetryHelper.ExecuteWithRetry(
        () => fileProcessor.ReadImage("images/photo.png"),
                                                retryCount: 3,
                                                initialDelay: delay,
                                                shouldRetry: retryPolicy);
    Console.WriteLine($"Результат: прочитано {image.Length} байт.\n");
}
catch (Exception ex)
{
    Console.WriteLine($"Операція остаточно невдала: {ex.Message}\n");
}
finally
{
    Console.WriteLine("[finally] Сценарій 1 завершено.\n");
}

// Сценарій 2: NetworkClient (2 невдачі -> успіх)
Console.WriteLine("=== Сценарій 2: DownloadImage (2 невдачі, потім успіх) ===");
var networkClient = new NetworkClient(failuresBeforeSuccess: 2);
try
{
    byte[] image = RetryHelper.ExecuteWithRetry(
        () => networkClient.DownloadImage("https://example.com/photo.jpg"),
                                                retryCount: 3,
                                                initialDelay: delay,
                                                shouldRetry: retryPolicy);
    Console.WriteLine($"Результат: завантажено {image.Length} байт.\n");
}
catch (Exception ex)
{
    Console.WriteLine($"Операція остаточно невдала: {ex.Message}\n");
}
finally
{
    Console.WriteLine("[finally] Сценарій 2 завершено.\n");
}

// Сценарій 3: вибіркова обробка (IOException не повторюється)
Console.WriteLine("=== Сценарій 3: shouldRetry — IOException не повторюється ===");
var corrupted = new FileProcessor();
try
{
    RetryHelper.ExecuteWithRetry(
        () => corrupted.ReadCorruptedImage("images/broken.png"),
                                 retryCount: 3,
                                 initialDelay: delay,
                                 shouldRetry: retryPolicy);
}
catch (IOException ex)
{
    Console.WriteLine($"Перехоплено IOException без повторів: {ex.Message}\n");
}
finally
{
    Console.WriteLine("[finally] Сценарій 3 завершено.\n");
}

// Сценарій 4: вичерпання спроб
Console.WriteLine("=== Сценарій 4: сервер недоступний, спроби вичерпано ===");
var deadServer = new NetworkClient(failuresBeforeSuccess: int.MaxValue);
try
{
    RetryHelper.ExecuteWithRetry(
        () => deadServer.DownloadImage("https://example.com/missing.jpg"),
                                 retryCount: 2,
                                 initialDelay: delay,
                                 shouldRetry: retryPolicy);
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Всі спроби вичерпано: {ex.Message}\n");
}
finally
{
    Console.WriteLine("[finally] Сценарій 4 завершено.");
}
