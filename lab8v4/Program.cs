using System;
using System.Collections.Generic;

// --- 1. Реалізація ієрархії класів ---

// Базовий клас пристрою
public class Device
{
    public string Brand { get; }

    public Device(string brand)
    {
        Brand = brand;
    }

    // Віртуальний метод, який повертає рядок із описом дії (для подальшої агрегації)
    public virtual string PerformAction()
    {
        return $"Пристрій {Brand} виконує базову дію.";
    }
}

// Похідний клас: Принтер
public class Printer : Device
{
    public int PagesPerMinute { get; }

    public Printer(string brand, int pagesPerMinute) : base(brand)
    {
        PagesPerMinute = pagesPerMinute;
    }

    public override string PerformAction()
    {
        return $"[Printer] {Brand} друкує сторінки зі швидкістю {PagesPerMinute} стор/хв.";
    }
}

// Похідний клас: Сканер
public class Scanner : Device
{
    public int DPI { get; }

    public Scanner(string brand, int dpi) : base(brand)
    {
        DPI = dpi;
    }

    public override string PerformAction()
    {
        return $"[Scanner] {Brand} сканує документ із роздільною здатністю {DPI} DPI.";
    }
}

// Похідний клас: Монітор
public class Monitor : Device
{
    public double ScreenSize { get; }

    public Monitor(string brand, double screenSize) : base(brand)
    {
        ScreenSize = screenSize;
    }

    public override string PerformAction()
    {
        return $"[Monitor] {Brand} відображає яскраве зображення на екрані {ScreenSize}\".";
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Створення колекції об'єктів базового типу
        List<Device> devices = new List<Device>
        {
            new Printer("HP LaserJet", 22),
            new Scanner("Epson Perfection", 4800),
            new Monitor("Dell UltraSharp", 27.0),
            new Printer("Canon PIXMA", 15),
            new Monitor("Asus ROG", 31.5)
        };

        // Список для агрегації результатів усіх виконаних дій
        List<string> actionHistory = new List<string>();

        Console.WriteLine("=== Виклик поліморфних методів у циклі ===");

        foreach (var device in devices)
        {
            // Динамічне зв'язування: викликається override метод конкретного похідного типу
            string result = device.PerformAction();
            
            // Виведення в консоль
            Console.WriteLine(result);
            
            // Агрегація: збираємо підсумковий список дій
            actionHistory.Add(result);
        }

        Console.WriteLine("\n=== Агрегація: Звіт про виконані дії ===");
        Console.WriteLine($"Всього пристроїв оброблено: {actionHistory.Count}");
        Console.WriteLine("Журнал виконаних операцій:");
        foreach (var action in actionHistory)
        {
            Console.WriteLine($" - {action}");
        }
    }
}
