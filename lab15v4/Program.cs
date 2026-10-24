using System;
using System.Collections.Generic;

namespace lab15v4
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №9 (Варіант 4) ===\n");

            HashSet<string> uniqueUrls = new HashSet<string>();
            Queue<string> requestQueue = new Queue<string>();
            Stack<string> navigationHistory = new Stack<string>();

            Console.WriteLine("--- Крок 1: Отримання та реєстрація запитів ---");
            string[] incomingRequests = {
                "https://rvc.edu.ua/news",
                "https://google.com",
                "https://rvc.edu.ua/news",
                "https://github.com"
            };

            foreach (var url in incomingRequests)
            {
                if (uniqueUrls.Add(url))
                {
                    Console.WriteLine($"[HashSet] Новий унікальний URL зареєстровано: {url}");
                    requestQueue.Enqueue(url);
                }
                else
                {
                    Console.WriteLine($"[HashSet] Дублікат проігноровано: {url}");
                }
            }

            Console.WriteLine("\n--- Крок 2: Обробка запитів з черги (FIFO) ---");
            while (requestQueue.Count > 0)
            {
                string currentUrl = requestQueue.Dequeue();
                Console.WriteLine($"[Queue] Обробляється запит: {currentUrl}");
                navigationHistory.Push(currentUrl);
            }

            Console.WriteLine("\n--- Крок 3: Навігація назад за допомогою історії стосу (LIFO) ---");
            if (navigationHistory.Count > 0)
            {
                Console.WriteLine($"[Stack] Поточна відкрита сторінка: {navigationHistory.Peek()}");
            }

            while (navigationHistory.Count > 1)
            {
                string poppedUrl = navigationHistory.Pop();
                Console.WriteLine($"[Stack] Натиснуто кнопку 'Назад'. Залишаємо сторінку: {poppedUrl}");
                Console.WriteLine($"[Stack] Повернулися на: {navigationHistory.Peek()}");
            }
            
            Console.WriteLine($"[Stack] Кінцева сторінка в історії: {navigationHistory.Pop()}");
        }
    }
}