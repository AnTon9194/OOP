using System;

namespace EncapsulationDemo
{
    // Демонстраційний клас з дотриманням інкапсуляції та валідації
    public class UserProfile
    {
        // Приватні поля (backing fields)
        private string _username = string.Empty;
        private int _age;

        // Властивість з валідацією в set-аксесорі
        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty or whitespaces.", nameof(value));
                _username = value.Trim();
            }
        }

        // Властивість з перевіркою діапазону
        public int Age
        {
            get => _age;
            set
            {
                if (value < 0 || value > 120)
                    throw new ArgumentOutOfRangeException(nameof(value), "Age must be between 0 and 120.");
                _age = value;
            }
        }

        // Властивість тільки для читання (Read-only)
        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        public UserProfile(string username, int age)
        {
            Username = username;
            Age = age;
        }

        public override string ToString() => $"User: {Username}, Age: {Age}, Created: {CreatedAt:yyyy-MM-dd}";
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрація інкапсуляції та валідації ===");
            try
            {
                var user = new UserProfile("  AlexCoder  ", 25);
                Console.WriteLine($"Успішно створено: {user}");

                Console.WriteLine("\nСпроба встановити некоректний вік (-5)...");
                user.Age = -5;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Валідація перехопила помилку]: {ex.Message}");
            }
        }
    }
}
