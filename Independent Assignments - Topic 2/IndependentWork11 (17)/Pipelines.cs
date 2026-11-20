using System.Diagnostics;

namespace IndependentWork11;

public static class Pipelines
{
    /// Пайплайн 1: топ-3 найпопулярніші продукти в кожній категорії.
    public static void TopProductsByCategory(List<Sale> sales, List<Product> products)
    {
        var sw = Stopwatch.StartNew();

        var result = sales
            .GroupBy(s => s.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQty = g.Sum(s => s.Quantity) })
            .Join(products, x => x.ProductId, p => p.Id,
                  (x, p) => new { p.Name, p.Category, x.TotalQty })
            .GroupBy(x => x.Category)
            .Select(g => new
            {
                Category = g.Key,
                Top = g.OrderByDescending(x => x.TotalQty).Take(3).ToList()
            })
            .OrderBy(x => x.Category)
            .ToList();

        sw.Stop();

        Console.WriteLine("--- Пайплайн 1: Топ-3 продукти за категоріями ---");
        foreach (var c in result)
        {
            Console.WriteLine($"{c.Category}:");
            foreach (var p in c.Top)
                Console.WriteLine($"   {p.Name,-14} {p.TotalQty,5} шт.");
        }
        Console.WriteLine($"Час: {sw.Elapsed.TotalMilliseconds:F3} мс\n");
    }

    /// Пайплайн 2: загальний дохід за містами клієнтів.
    public static void RevenueByCity(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        var sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id,
                  (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
            .Join(customers, x => x.CustomerId, c => c.Id,
                  (x, c) => new { c.City, x.Revenue })
            .GroupBy(x => x.City)
            .Select(g => new { City = g.Key, Total = g.Sum(x => x.Revenue) })
            .OrderByDescending(x => x.Total)
            .ToList();

        sw.Stop();

        Console.WriteLine("--- Пайплайн 2: Дохід за містами ---");
        foreach (var r in result)
            Console.WriteLine($"{r.City,-10} {r.Total,12:N2}");
        Console.WriteLine($"Час: {sw.Elapsed.TotalMilliseconds:F3} мс\n");
    }

    /// Пайплайн 3: продажі за роками та місяцями.
    public static void SalesByMonth(List<Sale> sales, List<Product> products)
    {
        var sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id,
                  (s, p) => new { s.SaleDate, s.Quantity, Revenue = s.Quantity * p.UnitPrice })
            .GroupBy(x => new { x.SaleDate.Year, x.SaleDate.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Units = g.Sum(x => x.Quantity),
                Revenue = g.Sum(x => x.Revenue)
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToList();

        sw.Stop();

        Console.WriteLine("--- Пайплайн 3: Продажі за місяцями ---");
        foreach (var r in result)
            Console.WriteLine($"{r.Year}-{r.Month:D2}  {r.Units,5} шт.  {r.Revenue,12:N2}");
        Console.WriteLine($"Час: {sw.Elapsed.TotalMilliseconds:F3} мс\n");
    }

    /// Пайплайн 4 (додатково): топ-5 клієнтів за витратами + їхня улюблена категорія.
    public static void TopCustomers(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        var sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id,
                  (s, p) => new { s.CustomerId, p.Category, Spent = s.Quantity * p.UnitPrice })
            .GroupBy(x => x.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                Spent = g.Sum(x => x.Spent),
                FavoriteCategory = g.GroupBy(x => x.Category)
                                    .OrderByDescending(cg => cg.Sum(x => x.Spent))
                                    .First().Key
            })
            .OrderByDescending(x => x.Spent)
            .Take(5)
            .Join(customers, x => x.CustomerId, c => c.Id,
                  (x, c) => new { c.Name, c.City, x.Spent, x.FavoriteCategory })
            .ToList();

        sw.Stop();

        Console.WriteLine("--- Пайплайн 4: Топ-5 клієнтів ---");
        foreach (var r in result)
            Console.WriteLine($"{r.Name,-13} {r.City,-9} {r.Spent,11:N2}  {r.FavoriteCategory}");
        Console.WriteLine($"Час: {sw.Elapsed.TotalMilliseconds:F3} мс\n");
    }
}
