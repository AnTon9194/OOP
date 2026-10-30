using System;

namespace lab5v1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №5 (Самостійна робота №3) ===");
            Console.WriteLine("Тема: Індексатори та перевантаження операторів\n");

            // 1. Створення об'єктів
            IntArray arr1 = new IntArray(10, 20, 30, 40);
            IntArray arr2 = new IntArray(1, 2, 3, 4);
            IntArray arr3 = new IntArray(10, 20, 30, 40);

            Console.WriteLine($"arr1: {arr1}");
            Console.WriteLine($"arr2: {arr2}");
            Console.WriteLine($"arr3: {arr3}\n");

            // 2. Демонстрація індексатора (читання та запис)
            Console.WriteLine("--- Робота з індексатором ---");
            Console.WriteLine($"arr1[2] (читання): {arr1[2]}");
            arr1[2] = 99;
            Console.WriteLine($"arr1[2] = 99 (після запису): {arr1}");
            arr1[2] = 30; // відновлюємо початкове значення
            Console.WriteLine($"Відновлено arr1: {arr1}\n");

            // 3. Демонстрація перевантаженого оператора +
            Console.WriteLine("--- Оператор додавання (+) ---");
            IntArray sum = arr1 + arr2;
            Console.WriteLine($"arr1 + arr2 = {sum}\n");

            // 4. Демонстрація операторів == та !=
            Console.WriteLine("--- Оператори порівняння (==, !=) ---");
            Console.WriteLine($"arr1 == arr3 : {arr1 == arr3}");
            Console.WriteLine($"arr1 != arr2 : {arr1 != arr2}");
            Console.WriteLine($"arr1 == arr2 : {arr1 == arr2}");
        }
    }
}
