using System;
using System.Collections.Generic;
using IndependentWork6.Notifications;
using IndependentWork6.FileProcessors;

namespace IndependentWork6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ДЕМОНСТРАЦІЯ СЦЕНАРІЮ 1: СИСТЕМА СПОВІЩЕНЬ (Інтерфейс) ===");
            List<INotifier> notifiers = new List<INotifier>
            {
                new EmailNotifier("user@example.com"),
                new SmsNotifier("+380671234567"),
                new PushNotifier("device_token_abc123")
            };

            string notificationMessage = "Важливе системне сповіщення!";
            foreach (INotifier notifier in notifiers)
            {
                notifier.Send(notificationMessage);
            }

            Console.WriteLine("\n=== ДЕМОНСТРАЦІЯ СЦЕНАРІЮ 2: ОБРОБКА ФАЙЛІВ (Абстрактний клас) ===");
            List<FileProcessor> processors = new List<FileProcessor>
            {
                new TextFileProcessor(),
                new XmlFileProcessor()
            };

            foreach (FileProcessor processor in processors)
            {
                processor.Process("document.data");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}
