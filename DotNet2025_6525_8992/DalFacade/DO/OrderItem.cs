namespace DO;

public record OrderItem(
    int OrderId,
    int ProductId,
    int Quantity,
    double PricePerUnit
)
{
    public OrderItem() : this(0, 0, 0, 0.0) { }
}