using System.Text.RegularExpressions;

namespace DictHashSetDemo;

/// <summary>Сценарій 3 (додатково): простий інвертований індекс слово -> документи.</summary>
public class SearchIndex
{
    private readonly Dictionary<string, HashSet<int>> _invertedIndex = new();

    public void AddDocument(int docId, string content)
    {
        foreach (Match m in Regex.Matches(content.ToLowerInvariant(), @"\w+"))
        {
            if (!_invertedIndex.TryGetValue(m.Value, out var docs))
            {
                docs = new HashSet<int>();
                _invertedIndex[m.Value] = docs;
            }
            docs.Add(docId);
        }
    }

    public HashSet<int> Search(string word) =>
        _invertedIndex.TryGetValue(word.ToLowerInvariant(), out var docs)
            ? new HashSet<int>(docs)
            : new HashSet<int>();

    public HashSet<int> SearchMultiple(params string[] words)
    {
        if (words.Length == 0) return new HashSet<int>();
        var result = Search(words[0]);
        foreach (var w in words.Skip(1))
        {
            result.IntersectWith(Search(w));
            if (result.Count == 0) break;
        }
        return result;
    }
}
