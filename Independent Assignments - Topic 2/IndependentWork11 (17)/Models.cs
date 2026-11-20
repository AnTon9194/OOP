namespace IndependentWork11;

public record Product(int Id, string Name, string Category, decimal UnitPrice);
public record Customer(int Id, string Name, string City);
public record Sale(int Id, int ProductId, int Quantity, DateTime SaleDate, int CustomerId);
