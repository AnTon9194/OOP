using System.Text.RegularExpressions;

namespace InvertedIndexDemo;

public static class TextProcessor
{
    private static readonly Regex WordRegex = new(@"\w+#?", RegexOptions.Compiled);

    private static readonly HashSet<string> StopWords = new()
    {
        "a", "an", "the", "is", "are", "was", "were",
        "in", "on", "at", "to", "for", "of", "and", "or"
    };

    /// <summary>
    /// Розбиває текст на слова, прибирає пунктуацію, переводить у нижній регістр.
    /// "Hello, world!" -> ["hello", "world"]; "C#," -> ["c#"].
    /// </summary>
    public static IEnumerable<string> Tokenize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Enumerable.Empty<string>();

        return WordRegex.Matches(text.ToLowerInvariant()).Select(m => m.Value);
    }

    /// <summary>Видаляє стоп-слова.</summary>
    public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words) =>
        words.Where(w => !StopWords.Contains(w));
}
