using DalApi;
namespace Dal;

public sealed class DalList : IDal
{

    public IProduct Product { get; } = new ProductImplementation();
    public ICustomer Customer { get; } = new CustomerImplementation();
    public ISale Sale { get; } = new SaleImplementation();

    public static IDal Instance { get; } = new DalList();

    private DalList() { }
}