namespace IndependentWork11;

public static class DataGenerator
{
    private static readonly string[] Categories = { "Electronics", "Books", "Home", "Sports", "Toys" };
    private static readonly string[] Cities = { "Kyiv", "Lviv", "Rivne", "Odesa", "Kharkiv", "Dnipro", "Lutsk", "Ternopil" };

    // Фіксований Seed -> результати відтворювані
    public static List<Product> Products(int count, Random rnd) =>
        Enumerable.Range(1, count)
            .Select(i => new Product(i, $"Product {i}", Categories[rnd.Next(Categories.Length)],
                                     Math.Round((decimal)(rnd.NextDouble() * 990 + 10), 2)))
            .ToList();

    public static List<Customer> Customers(int count, Random rnd) =>
        Enumerable.Range(1, count)
            .Select(i => new Customer(i, $"Customer {i}", Cities[rnd.Next(Cities.Length)]))
            .ToList();

    public static List<Sale> Sales(int count, int productCount, int customerCount, Random rnd) =>
        Enumerable.Range(1, count)
            .Select(i => new Sale(i, rnd.Next(1, productCount + 1), rnd.Next(1, 11),
                                  new DateTime(2024, 1, 1).AddDays(rnd.Next(0, 730)),
                                  rnd.Next(1, customerCount + 1)))
            .ToList();
}
