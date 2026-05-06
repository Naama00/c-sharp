namespace DO;

public record Order(
    int Id,
    int CustomerId,
    DateTime OrderDate,
    double TotalPrice
)
{
    public Order() : this(0, 0, DateTime.MinValue, 0.0) { }
}