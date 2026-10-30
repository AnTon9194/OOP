using System;
using System.Collections.Generic;

namespace lab3v4
{
    public class MemoryCache : IDisposable
    {
        private Dictionary<string, string> _cacheData;
        private bool _isActive;
        private bool _disposed = false;

        public MemoryCache()
        {
            _cacheData = new Dictionary<string, string>();
            _isActive = true;
            Console.WriteLine("[Constructor]: MemoryCache створено. Ресурси виділено.");
        }

        public void Set(string key, string value)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(MemoryCache), "Кеш уже знищено!");
            _cacheData[key] = value;
            Console.WriteLine($"[Cache]: Записано пара {key} -> {value}");
        }

        public string Get(string key)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(MemoryCache), "Кеш уже знищено!");
            
            if (_cacheData.TryGetValue(key, out string? value))
            {
                return value;
            }
            return "Ключ не знайдено";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_cacheData != null)
                    {
                        _cacheData.Clear();
                        Console.WriteLine("[Dispose(true)]: Очищено керовану колекцію _cacheData.");
                    }
                }

                if (_isActive)
                {
                    _isActive = false;
                    Console.WriteLine("[Dispose]: Деактивація кешу (_isActive = false). Ресурси звільнено.");
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
            Console.WriteLine("[Dispose()]: Викликано явне очищення. Фіналізацію скасовано.");
        }

        ~MemoryCache()
        {
            Dispose(false);
            Console.WriteLine("[Destructor ~MemoryCache]: Працює фіналізатор GC.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ДЕМОНСТРАЦІЯ ЖИТТЄВОГО ЦИКЛУ ОБ'ЄКТА ===\n");

            Console.WriteLine("--- Сценарій 1: Конструкція using ---");
            using (MemoryCache cache1 = new MemoryCache())
            {
                cache1.Set("user_1", "Адміністратор");
                Console.WriteLine($"Отримано: {cache1.Get("user_1")}");
            }
            Console.WriteLine("Блок using завершено.\n");

            Console.WriteLine("--- Сценарій 2: Явний виклик Dispose() ---");
            MemoryCache cache2 = new MemoryCache();
            cache2.Set("session_token", "XYZ12345");
            cache2.Dispose();
            Console.WriteLine("Явне очищення завершено.\n");

            Console.WriteLine("--- Сценарій 3: Очищення через Деструктор (GC) ---");
            CreateAndAbandonObject();
            
            Console.WriteLine("Об'єкт втратив посилання. Викликаємо збирання сміття...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            Console.WriteLine("\nПрограма завершила роботу.");
        }

        static void CreateAndAbandonObject()
        {
            MemoryCache cache3 = new MemoryCache();
            cache3.Set("temp_data", "Буде видалено GC");
        }
    }
}
