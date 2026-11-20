namespace InvertedIndexDemo;

public class InvertedIndex
{
    // Слово -> множина ID документів
    private readonly Dictionary<string, HashSet<int>> _index = new();
    // ID -> документ
    private readonly Dictionary<int, Document> _documents = new();

    public int DocumentCount => _documents.Count;
    public int UniqueWords => _index.Keys.Count;

    /// <summary>Додає документ до індексу.</summary>
    public void AddDocument(Document doc)
    {
        ArgumentNullException.ThrowIfNull(doc);
        _documents[doc.Id] = doc;

        foreach (var word in TextProcessor.Tokenize(doc.Content))
        {
            if (!_index.TryGetValue(word, out var ids))
            {
                ids = new HashSet<int>();
                _index[word] = ids;
            }
            ids.Add(doc.Id);
        }
    }

    /// <summary>Документи, що містять слово.</summary>
    public List<Document> SearchSingleWord(string word)
    {
        var token = TextProcessor.Tokenize(word).FirstOrDefault();
        if (token == null || !_index.TryGetValue(token, out var ids))
            return new List<Document>();

        return ToDocuments(ids);
    }

    /// <summary>Документи, що містять ВСІ слова (AND, IntersectWith).</summary>
    public List<Document> SearchMultipleWords(params string[] words)
    {
        var tokens = words.SelectMany(TextProcessor.Tokenize).ToList();
        if (tokens.Count == 0)
            return new List<Document>();

        HashSet<int>? result = null;
        foreach (var token in tokens)
        {
            if (!_index.TryGetValue(token, out var ids))
                return new List<Document>(); // слова немає ніде -> AND порожній

            if (result == null)
                result = new HashSet<int>(ids); // копія, щоб не зіпсувати індекс
            else
                result.IntersectWith(ids);

            if (result.Count == 0)
                break;
        }
        return ToDocuments(result!);
    }

    /// <summary>Документи, що містять БУДЬ-ЯКЕ слово (OR, UnionWith).</summary>
    public List<Document> SearchAnyWord(params string[] words)
    {
        var result = new HashSet<int>();
        foreach (var token in words.SelectMany(TextProcessor.Tokenize))
        {
            if (_index.TryGetValue(token, out var ids))
                result.UnionWith(ids);
        }
        return ToDocuments(result);
    }

    private List<Document> ToDocuments(IEnumerable<int> ids) =>
        ids.OrderBy(id => id).Select(id => _documents[id]).ToList();
}
