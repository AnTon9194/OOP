using System;
using lab12v4;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Лабораторна робота №7 (Варіант 4) ===");

        var repo = new InMemoryRepository<Category>();

        repo.Add(new Category { Id = 1, Name = "Електроніка", Description = "Гаджети та побутова техніка" });
        repo.Add(new Category { Id = 2, Name = "Книги", Description = "Художня та наукова література" });

        Console.WriteLine("\nВсі категорії:");
        foreach (var cat in repo.GetAll())
        {
            Console.WriteLine($"[{cat.Id}] {cat.Name}: {cat.Description}");
        }

        var toUpdate = new Category { Id = 1, Name = "Смартфони та ПК", Description = "Оновлений опис електроніки" };
        repo.Update(toUpdate);

        var updated = repo.GetById(1);
        Console.WriteLine($"\nОновлена категорія 1: {updated?.Name} -> {updated?.Description}");

        repo.Delete(2);
        Console.WriteLine($"\nКількість після видалення: {System.Linq.Enumerable.Count(repo.GetAll())}");
    }
}