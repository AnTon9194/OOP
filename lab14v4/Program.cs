using System;
using System.Collections.Generic;
using lab14v4;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Лабораторна робота №8 (Варіант 4) ===\n");

        Console.WriteLine("--- Колекція 1 (Рядки) ---");
        List<string> cities = new List<string> { "Рівне", "Київ", "Львів" };
        CollectionHelper<string>.ForEach(cities, city => Console.WriteLine($"Місто: {city.ToUpper()}"));

        Console.WriteLine("\n--- Колекція 2 (Продукти - Збільшення ціни на 10%) ---");
        List<Product> products = new List<Product>
        {
            new Product { Name = "Ноутбук", Price = 30000 },
            new Product { Name = "Мишка", Price = 500 }
        };

        Console.WriteLine("До модифікації:");
        CollectionHelper<Product>.ForEach(products, p => Console.WriteLine($"{p.Name}: {p.Price} грн"));

        CollectionHelper<Product>.ForEach(products, p => p.Price *= 1.10m);

        Console.WriteLine("\nПісля модифікації (+10%):");
        CollectionHelper<Product>.ForEach(products, p => Console.WriteLine($"{p.Name}: {p.Price:F2} грн"));
    }
}