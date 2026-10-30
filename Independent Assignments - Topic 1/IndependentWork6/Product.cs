namespace IndependentWork5
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"[Product] ID: {Id}, Назва: '{Name}', Ціна: {Price:C}";
        }
    }
}
