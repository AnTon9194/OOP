using System;

namespace IndependentWork1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Самостійна робота №1: Базовий синтаксис C# і оголошення класів ===");
            Console.WriteLine();

            // 1. Демонстрація роботи класу Rectangle
            Console.WriteLine("--- 1. Демонстрація класу Rectangle ---");
            Rectangle rect = new Rectangle(5.5, 4.0);
            rect.PrintInfo();
            Console.WriteLine($"Зміна ширини прямокутника на 10.0...");
            rect.Width = 10.0;
            rect.PrintInfo();
            Console.WriteLine();

            // 2. Демонстрація роботи класу BankAccount
            Console.WriteLine("--- 2. Демонстрація класу BankAccount ---");
            BankAccount account = new BankAccount("UA1234567890", 1500.00);
            Console.WriteLine($"Створено рахунок {account.AccountNumber} з початковим балансом {account.Balance:F2} грн.");
            account.Deposit(500.00);
            account.Withdraw(300.00);
            account.Withdraw(2000.00); // Перевірка спроби зняти більше, ніж є на рахунку
            Console.WriteLine();

            // 3. Демонстрація роботи класу Employee
            Console.WriteLine("--- 3. Демонстрація класу Employee ---");
            Employee emp = new Employee("Петренко Іван Васильович", 25000.00);
            emp.PrintInfo();
            double bonusPercentage = 15.0;
            double bonusAmount = emp.CalculateBonus(bonusPercentage);
            Console.WriteLine($"Премія ({bonusPercentage}%): {bonusAmount:F2} грн");
            Console.WriteLine($"Загальна виплата з премією: {(emp.MonthlySalary + bonusAmount):F2} грн");
            Console.WriteLine();

            Console.WriteLine("=== Програму успішно виконано ===");
        }
    }
}
