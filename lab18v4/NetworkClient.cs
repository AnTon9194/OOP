namespace lab18vN;

/// <summary>
/// Імітує мережеві запити.
/// </summary>
public class NetworkClient
{
    private readonly int _failuresBeforeSuccess;
    private int _calls;

    public NetworkClient(int failuresBeforeSuccess = 2)
    {
        _failuresBeforeSuccess = failuresBeforeSuccess;
    }

    /// <summary>
    /// Завантажує зображення. Перші N викликів кидають HttpRequestException, далі — успіх.
    /// </summary>
    public byte[] DownloadImage(string url)
    {
        _calls++;
        if (_calls <= _failuresBeforeSuccess)
            throw new HttpRequestException($"Не вдалося з'єднатися з '{url}' (виклик №{_calls}).");

        return new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46 };
    }
}
