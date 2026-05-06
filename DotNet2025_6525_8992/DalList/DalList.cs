using DalApi;
namespace Dal;
internal sealed class DalList : IDal
{
    public IProduct Product { get; } = new ProductImplementation();
    public ICustomer Customer { get; } = new CustomerImplementation();
    public ISale Sale { get; } = new SaleImplementation();
    public IOrder Order { get; } = new OrderImplementation();
    public IOrderItem OrderItem { get; }

    private static readonly DalList instance = new DalList();
    public static IDal Instance => instance;
    private DalList() { }
}