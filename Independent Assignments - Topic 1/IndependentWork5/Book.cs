namespace IndependentWork4;

public class Book : LibraryItem
{
    private string _author;

    public string Author => _author;

    public Book(string title, int publicationYear, string uniqueId, string author)
        : base(title, publicationYear, uniqueId)
    {
        _author = author;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"   Автор: {Author}");
    }
}
