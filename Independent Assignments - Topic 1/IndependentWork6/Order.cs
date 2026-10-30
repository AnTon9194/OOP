using System.Collections.Generic;
using System.Linq;

namespace IndependentWork5
{
    public class Order
    {
        public int OrderId { get; set; }
        public Customer OrderCustomer { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

        public Order(int orderId, Customer customer)
        {
            OrderId = orderId;
            OrderCustomer = customer;
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public decimal GetTotalSum()
        {
            return Products.Sum(p => p.Price);
        }

        public override string ToString()
        {
            return $"[Order] №{OrderId}, Клієнт: {OrderCustomer.FullName}, Кількість товарів: {Products.Count}, Сума: {GetTotalSum():C}";
        }
    }
}
