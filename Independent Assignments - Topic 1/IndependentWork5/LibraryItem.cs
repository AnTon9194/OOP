namespace IndependentWork4;

public class LibraryItem
{
    private string _title;
    private int _publicationYear;
    private string _uniqueId;

    public string Title => _title;
    public int PublicationYear => _publicationYear;
    public string UniqueId => _uniqueId;

    public LibraryItem(string title, int publicationYear, string uniqueId)
    {
        _title = title;
        _publicationYear = publicationYear;
        _uniqueId = uniqueId;
    }

    public virtual void DisplayDetails()
    {
        Console.WriteLine($"[ID: {UniqueId}] \"{Title}\" ({PublicationYear})");
    }
}
