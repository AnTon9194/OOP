namespace IndependentWork4;

public class Journal : LibraryItem
{
    private int _issueNumber;

    public int IssueNumber => _issueNumber;

    public Journal(string title, int publicationYear, string uniqueId, int issueNumber)
        : base(title, publicationYear, uniqueId)
    {
        _issueNumber = issueNumber;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"   Номер випуску: №{IssueNumber}");
    }
}
