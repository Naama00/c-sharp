namespace BL.BO;

public class Order
{
    public int Id { get; init; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalPrice { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double PricePerUnit { get; set; }
}