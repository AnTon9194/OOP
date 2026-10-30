namespace IndependentWork4;

public class AudioBook : Book
{
    private string _narrator;

    public string Narrator => _narrator;

    public AudioBook(string title, int publicationYear, string uniqueId, string author, string narrator)
        : base(title, publicationYear, uniqueId, author)
    {
        _narrator = narrator;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"   Диктор/Оповідач: {Narrator}");
    }
}
