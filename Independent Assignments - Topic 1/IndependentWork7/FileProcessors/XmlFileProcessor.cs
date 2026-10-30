using System;

namespace IndependentWork6.FileProcessors
{
    public class XmlFileProcessor : FileProcessor
    {
        public XmlFileProcessor() : base("XML") { }

        public override void ProcessContent(string filePath)
        {
            Console.WriteLine($"[Логіка XML]: Парсинг XML-дерева, валідація схем та атрибутів для '{filePath}'.");
        }
    }
}
