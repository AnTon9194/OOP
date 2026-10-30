using System;
using System.Collections.Generic;

namespace IndependentWork7;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== Демонстрація роботи узагальненого FIFO Cache<TKey, TValue> ===");

        // Створення кешу ємністю 3 елементи
        var cache = new Cache<string, string>(3);

        Console.WriteLine("\n--- Додавання 3 елементів (A, B, C) ---");
        cache.Add("key1", "Value A");
        cache.Add("key2", "Value B");
        cache.Add("key3", "Value C");

        Console.WriteLine($"Поточний Count: {cache.Count}");
        Console.WriteLine($"key1: {cache.Get("key1")}");
        Console.WriteLine($"key2: {cache.Get("key2")}");
        Console.WriteLine($"key3: {cache.Get("key3")}");

        Console.WriteLine("\n--- Додавання 4-го елемента (D), витіснення key1 (FIFO) ---");
        cache.Add("key4", "Value D");

        Console.WriteLine($"Поточний Count: {cache.Count}");
        Console.WriteLine($"Чи містить key1: {cache.ContainsKey("key1")}");
        Console.WriteLine($"Чи містить key4: {cache.ContainsKey("key4")}");

        try
        {
            Console.WriteLine("Спроба отримати key1...");
            cache.Get("key1");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"Перехоплено очікуваний виняток: {ex.Message}");
        }

        Console.WriteLine("\n=== Демонстрацію завершено успішно ===");
    }
}
