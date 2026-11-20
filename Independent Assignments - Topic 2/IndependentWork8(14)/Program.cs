using DictHashSetDemo;

Console.WriteLine("=== Сценарій 1: FileMetadataCache ===");
var cache = new FileMetadataCache();
cache.Add(@"C:\docs\report.pdf", new FileMetadata
{
    FileName = "report.pdf", Size = 20480,
    LastModified = new DateTime(2026, 11, 1), Hash = "a1b2c3"
});
Console.WriteLine($"Contains (інший регістр): {cache.Contains(@"c:\DOCS\Report.pdf")}");
Console.WriteLine($"Get: {cache.Get(@"C:\docs\report.pdf")?.Hash}");
Console.WriteLine($"Remove: {cache.Remove(@"C:\docs\report.pdf")}, Count = {cache.Count}");

Console.WriteLine("\n=== Сценарій 2: TagManager ===");
var tags = new TagManager();
tags.AddTag("post1", "csharp"); tags.AddTag("post1", "linq"); tags.AddTag("post1", "dotnet");
tags.AddTag("post2", "CSharp"); tags.AddTag("post2", "async"); tags.AddTag("post2", "dotnet");
Console.WriteLine("Унікальні теги: " + string.Join(", ", tags.GetAllUniqueTags().OrderBy(t => t)));
Console.WriteLine("Спільні post1/post2: " + string.Join(", ", tags.FindCommonTags("post1", "post2").OrderBy(t => t)));
Console.WriteLine("Лише в post1: " + string.Join(", ", tags.FindUniqueTags("post1", "post2").OrderBy(t => t)));

Console.WriteLine("\n=== Сценарій 3: SearchIndex ===");
var index = new SearchIndex();
index.AddDocument(1, "C# is a great programming language");
index.AddDocument(2, "Java is also a programming language");
index.AddDocument(3, "C and Java are popular");
Console.WriteLine("Search 'java': " + string.Join(", ", index.Search("java").OrderBy(x => x)));
Console.WriteLine("SearchMultiple 'programming language': " +
    string.Join(", ", index.SearchMultiple("programming", "language").OrderBy(x => x)));
