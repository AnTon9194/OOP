using IndependentWork11;

var rnd = new Random(42);
var products = DataGenerator.Products(60, rnd);
var customers = DataGenerator.Customers(80, rnd);
var sales = DataGenerator.Sales(500, products.Count, customers.Count, rnd);

Console.WriteLine($"Дані: {products.Count} продуктів, {customers.Count} клієнтів, {sales.Count} продажів\n");

Pipelines.TopProductsByCategory(sales, products);
Pipelines.RevenueByCity(sales, products, customers);
Pipelines.SalesByMonth(sales, products);
Pipelines.TopCustomers(sales, products, customers);
