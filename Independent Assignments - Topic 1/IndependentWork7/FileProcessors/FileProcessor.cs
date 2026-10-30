using System;

namespace IndependentWork6.FileProcessors
{
    public abstract class FileProcessor
    {
        public string FileType { get; protected set; }

        protected FileProcessor(string fileType)
        {
            FileType = fileType;
        }

        // Загальний метод відкриття файлу (спільна логіка)
        public virtual void OpenFile(string filePath)
        {
            Console.WriteLine($"Відкриття {FileType} файлу: '{filePath}'...");
        }

        // Абстрактний метод обробки вмісту (унікальна логіка для кожного типу)
        public abstract void ProcessContent(string filePath);

        // Загальний метод закриття файлу (спільна логіка)
        public virtual void CloseFile(string filePath)
        {
            Console.WriteLine($"Закриття {FileType} файлу: '{filePath}'.\n");
        }

        // Шаблонний метод, що визначає алгоритм обробки файлу
        public void Process(string filePath)
        {
            OpenFile(filePath);
            ProcessContent(filePath);
            CloseFile(filePath);
        }
    }
}
