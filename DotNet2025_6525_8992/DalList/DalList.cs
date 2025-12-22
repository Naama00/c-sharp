using DalApi;
namespace Dal;

public sealed class DalList : IDal
{
    // Singletons או אתחול פשוט של מחלקות המימוש
    public IProduct Product => new ProductImplementation();
    public ICustomer Customer => new CustomerImplementation();
    public ISale Sale => new SaleImplementation();

   
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
}