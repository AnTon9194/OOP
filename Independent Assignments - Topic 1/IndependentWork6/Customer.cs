namespace IndependentWork5
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public Customer(int id, string fullName, string email)
        {
            Id = id;
            FullName = fullName;
            Email = email;
        }

        public override string ToString()
        {
            return $"[Customer] ID: {Id}, ПІБ: {FullName}, Email: {Email}";
        }
    }
}
