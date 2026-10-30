using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IndependentWork5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("==================================================");
            Console.WriteLine("      Самостійна робота №6: Поліморфізм в .NET    ");
            Console.WriteLine("==================================================\n");

            // -----------------------------------------------------------------
            // Сценарій 1: Поліморфізм через базовий клас System.Object
            // -----------------------------------------------------------------
            Console.WriteLine(">>> 1. Дослідження поліморфізму через System.Object <<<");
            
            Product product = new Product(101, "Ноутбук ASUS", 35000m);
            Customer customer = new Customer(1, "Олександр Ковальчук", "alex@example.com");
            Order order = new Order(5001, customer);
            order.AddProduct(product);
            order.AddProduct(new Product(102, "Мишка бездротова", 850m));

            // Поліморфний контейнер List<object>
            List<object> objectsList = new List<object>
            {
                product,
                customer,
                order,
                "Простий рядок (System.String)",
                12345
            };

            Console.WriteLine("Перебір List<object> та виклики ToString():");
            foreach (object obj in objectsList)
            {
                // Поліморфний виклик перевантаженого / перевизначеного методу ToString()
                Console.WriteLine($"Тип: {obj.GetType().Name,-15} | Output: {obj.ToString()}");
            }
            Console.WriteLine();

            // -----------------------------------------------------------------
            // Сценарій 2: Поліморфізм через інтерфейс IEnumerable
            // -----------------------------------------------------------------
            Console.WriteLine(">>> 2. Дослідження поліморфізму через IEnumerable<T> <<<");

            MyCustomCollection customCollection = new MyCustomCollection();
            customCollection.Add("Перший елемент колекції");
            customCollection.Add("Другий елемент колекції");
            customCollection.Add("Третій елемент колекції");

            Console.WriteLine("Перебір MyCustomCollection за допомогою циклу foreach:");
            foreach (string item in customCollection)
            {
                Console.WriteLine($" -> {item}");
            }
            Console.WriteLine();

            // -----------------------------------------------------------------
            // Сценарій 3: Поліморфізм через абстрактний клас System.IO.Stream
            // -----------------------------------------------------------------
            Console.WriteLine(">>> 3. Дослідження поліморфізму через абстрактний Stream <<<");

            // Створення тестового файла для FileStream (щоб розв'язок був самодостатнім)
            string testFilePath = "test_data.txt";
            File.WriteAllText(testFilePath, "Тестові дані для перевірки роботи FileStream у поліморфному методі ProcessStream.", Encoding.UTF8);

            try
            {
                // 3a. Робота з FileStream
                using (FileStream fileStream = new FileStream(testFilePath, FileMode.Open, FileAccess.Read))
                {
                    StreamProcessor.ProcessStream(fileStream);
                }

                // 3b. Робота з MemoryStream
                byte[] memoryBytes = Encoding.UTF8.GetBytes("Тестовий текст у оперативній пам'яті (MemoryStream).");
                using (MemoryStream memoryStream = new MemoryStream(memoryBytes))
                {
                    StreamProcessor.ProcessStream(memoryStream);
                }
            }
            finally
            {
                // Очищення тимчасового файлу після завершення
                if (File.Exists(testFilePath))
                {
                    File.Delete(testFilePath);
                }
            }

            Console.WriteLine("==================================================");
            Console.WriteLine("Виконання програми завершено успішно.");
            Console.WriteLine("==================================================");
        }
    }
}
