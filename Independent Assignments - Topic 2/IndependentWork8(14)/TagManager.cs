namespace DictHashSetDemo;

/// <summary>Теги статей блогу: ArticleId -> множина тегів (без урахування регістру).</summary>
public class TagManager
{
    private readonly Dictionary<string, HashSet<string>> _articleTags = new();

    public void AddTag(string articleId, string tag)
    {
        if (!_articleTags.TryGetValue(articleId, out var tags))
        {
            tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _articleTags[articleId] = tags;
        }
        tags.Add(tag);
    }

    public void RemoveTag(string articleId, string tag)
    {
        if (_articleTags.TryGetValue(articleId, out var tags))
            tags.Remove(tag);
    }

    /// <summary>Усі унікальні теги по всіх статтях (UnionWith).</summary>
    public HashSet<string> GetAllUniqueTags()
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var tags in _articleTags.Values)
            result.UnionWith(tags);
        return result;
    }

    /// <summary>Спільні теги двох статей (IntersectWith).</summary>
    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        if (!_articleTags.TryGetValue(articleId1, out var first) ||
            !_articleTags.TryGetValue(articleId2, out var second))
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var result = new HashSet<string>(first, StringComparer.OrdinalIgnoreCase);
        result.IntersectWith(second);
        return result;
    }

    /// <summary>Теги, які є в першій статті, але відсутні в другій (ExceptWith).</summary>
    public HashSet<string> FindUniqueTags(string articleId, string otherArticleId)
    {
        var result = _articleTags.TryGetValue(articleId, out var first)
            ? new HashSet<string>(first, StringComparer.OrdinalIgnoreCase)
            : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (_articleTags.TryGetValue(otherArticleId, out var second))
            result.ExceptWith(second);
        return result;
    }
}
