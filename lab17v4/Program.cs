using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab17v4
{
    // ===== Сутності =====
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Project> Projects { get; set; } = new();
    }

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Employee> Employees { get; set; } = new();
    }

    // ===== Методи розширення =====
    public static class CompanyExtensions
    {
        // Custom Extension: загальна кількість співробітників у всіх компаніях
        public static int TotalEmployees(this IEnumerable<Company> companies)
        {
            if (companies == null) throw new ArgumentNullException(nameof(companies));
            return companies.Sum(c => c.Employees.Count);
        }

        // Допоміжний узагальнений метод пагінації (Skip/Take), сторінки нумеруються з 1
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber));
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }

    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<Company> companies = CreateTestData();

            // ---------- 1. SelectMany ----------
            Console.WriteLine("=== 1. Усі проєкти всіх співробітників усіх компаній ===");
            List<Project> allProjects = companies
            .SelectMany(c => c.Employees)      // компанії -> співробітники
            .SelectMany(e => e.Projects)       // співробітники -> проєкти
            .ToList();

            foreach (Project p in allProjects)
                Console.WriteLine($"  [{p.Id}] {p.Title}");
            Console.WriteLine($"Разом проєктів: {allProjects.Count}");

            // Для порівняння: Select дає колекцію колекцій
            var nested = companies.SelectMany(c => c.Employees).Select(e => e.Projects);
            Console.WriteLine($"(Select без flatten дав би {nested.Count()} вкладених списків, а не {allProjects.Count} проєктів)");

            // ---------- 2. Пагінація ----------
            Console.WriteLine("\n=== 2. Пагінація: по 2 компанії на сторінку ===");
            const int pageSize = 2;
            int totalPages = (int)Math.Ceiling(companies.Count / (double)pageSize);

            for (int page = 1; page <= totalPages; page++)
            {
                Console.WriteLine($"--- Сторінка {page} з {totalPages} ---");
                foreach (Company c in companies.Page(page, pageSize))
                    Console.WriteLine($"  {c.Id}. {c.Name} (співробітників: {c.Employees.Count})");
            }

            // ---------- 3. Custom Extension ----------
            Console.WriteLine("\n=== 3. Extension TotalEmployees() ===");
            Console.WriteLine($"Загальна кількість співробітників: {companies.TotalEmployees()}");
            Console.WriteLine($"Для перших двох компаній: {companies.Take(2).TotalEmployees()}");
        }

        private static List<Company> CreateTestData()
        {
            // Проєкти (20 шт.)
            var p = Enumerable.Range(1, 20)
            .Select(i => new Project { Id = i, Title = $"Project {i}" })
            .ToArray();

            Employee E(int id, string name, params int[] projectIds) => new()
            {
                Id = id,
                Name = name,
                Projects = projectIds.Select(i => p[i - 1]).ToList()
            };

            return new List<Company>
            {
                new() { Id = 1, Name = "TechNova", Employees = new()
                    { E(1, "Олександр Коваль", 1, 2), E(2, "Марія Шевченко", 3) } },
                    new() { Id = 2, Name = "SoftLine", Employees = new()
                        { E(3, "Іван Бондар", 4, 5, 6), E(4, "Олена Мельник", 7) } },
                        new() { Id = 3, Name = "DataHub", Employees = new()
                            { E(5, "Андрій Ткачук", 8, 9), E(6, "Наталія Кравчук", 10, 11) } },
                            new() { Id = 4, Name = "CloudWorks", Employees = new()
                                { E(7, "Сергій Олійник", 12, 13), E(8, "Ірина Поліщук", 14, 15) } },
                                new() { Id = 5, Name = "GreenSoft", Employees = new()
                                    { E(9, "Василь Гончар", 16, 17), E(10, "Юлія Лисенко", 18, 19, 20) } }
            };
        }
    }
}
