using System;
using System.Collections.Generic;

namespace IndependentWork4;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Самостійна робота №5: Демонстрація ієрархії об'єктів ===\n");

        List<LibraryItem> items = new List<LibraryItem>
        {
            new Book("Кобзар", 1840, "LIB-B001", "Тарас Шевченко"),
            new Journal("Наука та фантастика", 2023, "LIB-J102", 45),
            new AudioBook("Тіні забутих предків", 1911, "LIB-A305", "Михайло Коцюбинський", "Олександр Бондаренко")
        };

        foreach (var item in items)
        {
            item.DisplayDetails();
            Console.WriteLine(new string('-', 40));
        }
    }
}
