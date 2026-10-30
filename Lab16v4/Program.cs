using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lab16v4
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public Author(int id, string name, string country)
        {
            Id = id;
            Name = name;
            Country = country;
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int Year { get; set; }

        public Book(int id, string title, int authorId, int year)
        {
            Id = id;
            Title = title;
            AuthorId = authorId;
            Year = year;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var authors = new List<Author>
            {
                new Author(1, "Stephen King", "USA"),
                new Author(2, "J.K. Rowling", "UK"),
                new Author(3, "George R.R. Martin", "USA"),
                new Author(4, "Haruki Murakami", "Japan"),
                new Author(5, "Agatha Christie", "UK"),
                new Author(6, "Robert Kurvitz", "EE")

            };

            var books = new List<Book>
            {
                new Book(1, "It", 1, 1986),
                new Book(2, "The Green Mile", 1, 1996),
                new Book(3, "11/22/63", 1, 2011),
                new Book(4, "The Outsider", 1, 2018),
                new Book(5, "Misery", 1, 1987),
                new Book(6, "Harry Potter and the Philosopher's Stone", 2, 1997),
                new Book(7, "Harry Potter and the Prisoner of Azkaban", 2, 1999),
                new Book(8, "A Game of Thrones", 3, 1996),
                new Book(9, "A Clash of Kings", 3, 1998),
                new Book(10, "Norwegian Wood", 4, 1987),
                new Book(11, "Kafka on the Shore", 4, 2002),
                new Book(12, "Murder on the Orient Express", 5, 1934),
                new Book(13, "And Then There Were None", 5, 1939),
                new Book(14, "Püha ja õudne lõhn", 6, 2013)
            };

            Stopwatch sw = new Stopwatch();

            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №10: LINQ В ДІЇ ===\n");

            Console.WriteLine("--- Запит 1: Книги Stephen King після 1990 року ---");
            sw.Restart();
            var stephenKingAuthor = authors.FirstOrDefault(a => a.Name == "Stephen King");
            var query1Method = books.Where(b => b.AuthorId == stephenKingAuthor?.Id && b.Year > 1990).ToList();
            sw.Stop();
            long t1M = sw.ElapsedTicks;
            Console.WriteLine($"[Методи] Знайдено книг: {query1Method.Count} (Час: {t1M} ticks)");

            sw.Restart();
            var query1Query = (from b in books
                               join a in authors on b.AuthorId equals a.Id
                               where a.Name == "Stephen King" && b.Year > 1990
                               select b).ToList();
            sw.Stop();
            long t1Q = sw.ElapsedTicks;
            Console.WriteLine($"[Запити] Знайдено книг: {query1Query.Count} (Час: {t1Q} ticks)");
            Console.WriteLine();

            Console.WriteLine("--- Запит 2: Об'єднання книг з авторами ---");
            sw.Restart();
            var query2Method = books.Join(authors,
                b => b.AuthorId,
                a => a.Id,
                (b, a) => new { BookTitle = b.Title, AuthorName = a.Name }).ToList();
            sw.Stop();
            long t2M = sw.ElapsedTicks;
            Console.WriteLine($"[Методи] Об'єднано рядків: {query2Method.Count} (Час: {t2M} ticks)");

            sw.Restart();
            var query2Query = (from b in books
                               join a in authors on b.AuthorId equals a.Id
                               select new { BookTitle = b.Title, AuthorName = a.Name }).ToList();
            sw.Stop();
            long t2Q = sw.ElapsedTicks;
            Console.WriteLine($"[Запити] Об'єднано рядків: {query2Query.Count} (Час: {t2Q} ticks)");
            Console.WriteLine();

            Console.WriteLine("--- Запит 3: Кількість книг для кожного автора ---");
            sw.Restart();
            var query3Method = books.GroupBy(b => b.AuthorId)
                                    .Select(g => new { AuthorId = g.Key, Count = g.Count() }).ToList();
            sw.Stop();
            long t3M = sw.ElapsedTicks;
            Console.WriteLine($"[Методи] Групування виконано (Час: {t3M} ticks)");

            sw.Restart();
            var query3Query = (from b in books
                               group b by b.AuthorId into g
                               select new { AuthorId = g.Key, Count = g.Count() }).ToList();
            sw.Stop();
            long t3Q = sw.ElapsedTicks;
            Console.WriteLine($"[Запити] Групування виконано (Час: {t3Q} ticks)");
            Console.WriteLine();

            Console.WriteLine("--- Запит 4: Найстарішу книгу ---");
            sw.Restart();
            int minYearMethod = books.Min(b => b.Year);
            var query4Method = books.FirstOrDefault(b => b.Year == minYearMethod);
            sw.Stop();
            long t4M = sw.ElapsedTicks;
            Console.WriteLine($"[Методи] Найстаріша: {query4Method?.Title} ({query4Method?.Year}) (Час: {t4M} ticks)");

            sw.Restart();
            var query4Query = (from b in books
                               where b.Year == (from b2 in books select b2.Year).Min()
                               select b).FirstOrDefault();
            sw.Stop();
            long t4Q = sw.ElapsedTicks;
            Console.WriteLine($"[Запити] Найстаріша: {query4Query?.Title} ({query4Query?.Year}) (Час: {t4Q} ticks)");
            Console.WriteLine();
        }
    }
}
