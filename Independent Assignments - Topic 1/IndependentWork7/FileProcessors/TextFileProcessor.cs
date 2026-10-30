using System;

namespace IndependentWork6.FileProcessors
{
    public class TextFileProcessor : FileProcessor
    {
        public TextFileProcessor() : base("Текстовий (TXT)") { }

        public override void ProcessContent(string filePath)
        {
            Console.WriteLine($"[Логіка TXT]: Зчитування рядків файлу '{filePath}' та аналіз тексту.");
        }
    }
}
