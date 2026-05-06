using DalApi;
namespace Dal;

internal sealed class DalXml : IDal
{
    // מימוש התכונות מהממשק IDal 
    public IProduct Product { get; } = new ProductDalXml();
    public ISale Sale { get; } = new SaleDalXML();
    public ICustomer Customer { get; } = new CustomerDalXML();
    public IOrderItem OrderItem { get; } = new OrderItemDalXML();
    public IOrder Order { get; } = new OrderDalXml();

    // מימוש ה-Singleton שה-Factory שלך מצפה לו
    private static readonly DalXml instance = new DalXml();
    public static DalXml Instance => instance;

    // בנאי פרטי
    private DalXml() { }
}