using System;
using System.IO;
using System.Text;

namespace IndependentWork5
{
    public static class StreamProcessor
    {
        /// <summary>
        /// Поліморфний метод обробки будь-якого потоку даних, що успадковується від System.IO.Stream.
        /// </summary>
        /// <param name="stream">Абстрактний базовий клас Stream</param>
        public static void ProcessStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            Console.WriteLine($"--- Обробка потоку типу: {stream.GetType().Name} ---");
            Console.WriteLine($"Чи підтримує читання: {stream.CanRead}");
            Console.WriteLine($"Чи підтримує позиціонування: {stream.CanSeek}");
            Console.WriteLine($"Довжина потоку: {stream.Length} байт(ів)");

            if (stream.CanSeek)
            {
                stream.Position = 0; // Скидаємо позицію на початок
            }

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            string content = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Прочитаний вміст ({bytesRead} байт):");
            Console.WriteLine($""{content}"");
            Console.WriteLine();
        }
    }
}
