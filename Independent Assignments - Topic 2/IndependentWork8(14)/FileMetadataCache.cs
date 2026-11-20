namespace DictHashSetDemo;

public class FileMetadata
{
    public string FileName { get; set; } = "";
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; } = "";
}

/// <summary>Кеш метаданих файлів. Ключ — шлях без урахування регістру.</summary>
public class FileMetadataCache
{
    private readonly Dictionary<string, FileMetadata> _cache =
        new(StringComparer.OrdinalIgnoreCase);

    public int Count => _cache.Count;

    public void Add(string path, FileMetadata metadata) => _cache[path] = metadata;

    public FileMetadata? Get(string path) =>
        _cache.TryGetValue(path, out var meta) ? meta : null;

    public bool Remove(string path) => _cache.Remove(path);

    public bool Contains(string path) => _cache.ContainsKey(path);
}
