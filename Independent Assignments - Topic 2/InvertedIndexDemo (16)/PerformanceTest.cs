using System.Diagnostics;

namespace InvertedIndexDemo;

public static class PerformanceTest
{
    private const int Repeats = 100; // одиничний пошук надто швидкий для Stopwatch

    public static void CompareSearchMethods(List<Document> documents, string searchWord)
    {
        var normalizedWord = TextProcessor.Tokenize(searchWord).First();

        // Побудова індексу (не входить у час пошуку)
        var buildSw = Stopwatch.StartNew();
        var index = new InvertedIndex();
        foreach (var doc in documents)
            index.AddDocument(doc);
        buildSw.Stop();

        // 1. Лінійний пошук (та сама нормалізація!)
        List<Document> linearResults = new();
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Repeats; i++)
        {
            linearResults = documents
                .Where(d => TextProcessor.Tokenize(d.Content).Contains(normalizedWord))
                .ToList();
        }
        sw.Stop();
        double linearMs = sw.Elapsed.TotalMilliseconds / Repeats;

        // 2. Inverted Index
        List<Document> indexResults = new();
        sw.Restart();
        for (int i = 0; i < Repeats; i++)
            indexResults = index.SearchSingleWord(searchWord);
        sw.Stop();
        double indexMs = sw.Elapsed.TotalMilliseconds / Repeats;

        Console.WriteLine($"Документів: {documents.Count}, слово: '{searchWord}'");
        Console.WriteLine($"  Побудова індексу: {buildSw.Elapsed.TotalMilliseconds:F3} мс (одноразово)");
        Console.WriteLine($"  Linear search:    {linearMs:F4} мс / запит");
        Console.WriteLine($"  Inverted Index:   {indexMs:F4} мс / запит");
        Console.WriteLine($"  Linear found: {linearResults.Count}, Index found: {indexResults.Count}"
                          + (linearResults.Count == indexResults.Count ? "  [OK]" : "  [НЕ ЗБІГАЄТЬСЯ!]"));
    }
}
