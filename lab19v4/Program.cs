using System;
using System.Text;

namespace lab19v4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var validator = new ValidatorService();

            Console.WriteLine("=== 1. Демонстрація роботи з власним винятком (ValidateProduct) ===\n");

            var validProduct = new Product { Name = "Ноутбук ASUS", Price = 25000m, StockQuantity = 5 };
            try
            {
                validator.ValidateProduct(validProduct);
                Console.WriteLine($"[Успіх] Продукт '{validProduct.Name}' успішно пройшов валідацію.\n");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"[Помилка] {ex.Message}");
            }

            var invalidProduct = new Product { Name = "ПК", Price = -100m, StockQuantity = -2 };
            try
            {
                validator.ValidateProduct(invalidProduct);
                Console.WriteLine($"[Успіх] Продукт '{invalidProduct.Name}' пройшов валідацію.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"[Перехоплено ValidationException]: {ex.Message}");
                foreach (var error in ex.Errors)
                {
                    Console.WriteLine($"  - {error}");
                }
            }

            Console.WriteLine("\n=== 2. Демонстрація роботи з патерном Result<T> (IsEmailValid) ===\n");

            string[] emailsToTest = { "student@university.edu.ua", "invalid-email.com", "" };

            foreach (var email in emailsToTest)
            {
                Result<bool> result = validator.IsEmailValid(email);

                if (result.IsSuccess)
                {
                    Console.WriteLine($"[Успіх] Email '{email}' валідний (Value: {result.Value}).");
                }
                else
                {
                    Console.WriteLine($"[Невдача] {result.Error}");
                }
            }
        }
    }
}
