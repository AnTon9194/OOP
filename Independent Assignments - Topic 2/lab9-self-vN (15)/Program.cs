using Lab9SelfV1;

// --- Колекція 1: List<int> ---
var numbers = new List<int> { 5, 12, 7, 3, 20, 8 };
Console.WriteLine("== List<int> ==");
Console.WriteLine("Квадрати: " + string.Join(", ", CollectionUtils.Map(numbers, n => n * n)));
Console.WriteLine("Парні:    " + string.Join(", ", CollectionUtils.Filter(numbers, n => n % 2 == 0)));
Console.WriteLine("Максимум: " + CollectionUtils.MaxBy(numbers, n => n));

// --- Колекція 2: List<string> ---
var words = new List<string> { "apple", "kiwi", "banana", "fig", "cherry" };
Console.WriteLine("\n== List<string> ==");
Console.WriteLine("Довжини:  " + string.Join(", ", CollectionUtils.Map(words, w => w.Length)));
Console.WriteLine("Довші за 4: " + string.Join(", ", CollectionUtils.Filter(words, w => w.Length > 4)));
Console.WriteLine("Найдовше слово: " + CollectionUtils.MaxBy(words, w => w.Length));
CollectionUtils.ForEach(words, w => Console.WriteLine($"  -> {w.ToUpper()}"));

// --- Колекція 3: List<Product> ---
var products = new List<Product>
{
    new() { Name = "Laptop",   Price = 25000 },
    new() { Name = "Mouse",    Price = 500 },
    new() { Name = "Keyboard", Price = 1500 }
};
Console.WriteLine("\n== List<Product> ==");
Console.WriteLine("Names: " + string.Join(", ", CollectionUtils.Map(products, p => p.Name)));

var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
Console.WriteLine($"Most expensive: {mostExpensive.Name} - {mostExpensive.Price}");

var expensive = CollectionUtils.Filter(products, p => p.Price > 1000);
Console.WriteLine("Expensive products:");
CollectionUtils.ForEach(expensive, p => Console.WriteLine($" - {p.Name}"));

CollectionUtils.ForEach(products, p => Console.WriteLine($"{p.Name}: {p.Price} грн"));

public class Product
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}
