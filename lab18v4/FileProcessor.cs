namespace lab18vN;

/// <summary>
/// Імітує роботу з файлами.
/// </summary>
public class FileProcessor
{
    private readonly int _failuresBeforeSuccess;
    private int _calls;

    public FileProcessor(int failuresBeforeSuccess = 3)
    {
        _failuresBeforeSuccess = failuresBeforeSuccess;
    }

    /// <summary>
    /// Читає зображення. Перші N викликів кидають FileNotFoundException (тимчасова помилка), далі — успіх.
    /// </summary>
    public byte[] ReadImage(string path)
    {
        _calls++;
        if (_calls <= _failuresBeforeSuccess)
            throw new FileNotFoundException($"Файл тимчасово недоступний (виклик №{_calls}).", path);

        // Умовний PNG-заголовок
        return new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
    }

    /// <summary>
    /// Імітує постійну помилку вводу/виводу (наприклад, пошкоджений диск) — повторювати немає сенсу.
    /// </summary>
    public byte[] ReadCorruptedImage(string path)
    {
        throw new IOException($"Помилка читання даних з '{path}': файл пошкоджено.");
    }
}
