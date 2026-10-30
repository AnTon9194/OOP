using System;

namespace IndependentWork2
{
    public class Product
    {
        // Приватні поля
        private readonly int _id;
        private readonly string _name;
        private readonly decimal _price;
        private readonly string _category;
        private readonly int _stockCount;

        // Публічні read-only властивості
        public int Id => _id;
        public string Name => _name;
        public decimal Price => _price;
        public string Category => _category;
        public int StockCount => _stockCount;

        // Конструктор 1 (основний)
        public Product(int id, string name, decimal price, string category, int stockCount)
        {
            _id = id;
            _name = name;
            _price = price;
            _category = category;
            _stockCount = stockCount;
        }

        // Конструктор 2 (для швидкого додавання товару)
        public Product(int id, string name, decimal price)
            : this(id, name, price, "Uncategorized", 0)
        {
        }

        // Конструктор 3 (конструктор копіювання)
        public Product(Product other)
            : this(other.Id, other.Name, other.Price, other.Category, other.StockCount)
        {
        }

        // Перевизначений метод ToString()
        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Price: {Price:C}, Category: {Category}, Stock: {StockCount}";
        }
    }
}
