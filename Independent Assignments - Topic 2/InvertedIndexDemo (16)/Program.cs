using InvertedIndexDemo;

var documents = new List<Document>
{
    new() { Id = 1, Title = "Introduction to C#",
            Content = "C# is a modern object-oriented programming language developed by Microsoft." },
    new() { Id = 2, Title = "Java Basics",
            Content = "Java is a popular programming language known for its portability and performance." },
    new() { Id = 3, Title = "Python Overview",
            Content = "Python is a high-level programming language with simple syntax." },
    new() { Id = 4, Title = "C# Advanced Features",
            Content = "C# supports LINQ, async/await, and many modern programming paradigms." },
    new() { Id = 5, Title = "Comparing Languages",
            Content = "C#, Java, and Python are among the most popular programming languages today." }
};

// ---- Частина 3: вимірювання продуктивності ----
Console.WriteLine("=== Performance ===");
PerformanceTest.CompareSearchMethods(documents, "python");
PerformanceTest.CompareSearchMethods(GenerateDocuments(documents, 10), "w42");
PerformanceTest.CompareSearchMethods(GenerateDocuments(documents, 1000), "w42");
Console.WriteLine();

// ---- Частина 4: інтерактивна демонстрація ----
var index = new InvertedIndex();
foreach (var d in documents) index.AddDocument(d);

Console.WriteLine($"Loaded {index.DocumentCount} documents");
Console.WriteLine($"Index contains {index.UniqueWords} unique words");
Console.WriteLine();

while (true)
{
    Console.Write("Enter search query (or 'exit'): ");
    var query = Console.ReadLine();
    if (query == null || query == "exit") break;

    var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    Console.WriteLine("\n--- Search Results (AND) ---");
    var results = index.SearchMultipleWords(words);
    foreach (var doc in results)
    {
        Console.WriteLine(doc);
        Console.WriteLine($"  {doc.Content.Substring(0, Math.Min(100, doc.Content.Length))}...");
    }
    Console.WriteLine($"\nFound {results.Count} documents\n");
}

// Генерація N документів: базові тексти + випадкові слова w0..w499 (фіксований Seed)
static List<Document> GenerateDocuments(List<Document> baseDocs, int count)
{
    var rnd = new Random(42);
    var list = new List<Document>(count);
    for (int i = 1; i <= count; i++)
    {
        var template = baseDocs[(i - 1) % baseDocs.Count];
        var extra = string.Join(" ", Enumerable.Range(0, 10).Select(_ => "w" + rnd.Next(500)));
        list.Add(new Document { Id = i, Title = $"{template.Title} #{i}", Content = $"{template.Content} {extra}" });
    }
    return list;
}
