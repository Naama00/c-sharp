using DalApi;
namespace Dal;

public sealed class DalXml : IDal
{
    // מימוש התכונות מהממשק IDal 
    public IProduct Product { get; } = new ProductDalXml();
    public ISale Sale { get; } = new SaleDalXML();
    public ICustomer Customer { get; } = new CustomerDalXML();
    public IOrder Order { get; } = new OrderDalXml();

    // מימוש ה-Singleton שה-Factory שלך מצפה לו
    private static readonly DalXml instance = new DalXml();
    public static DalXml Instance => instance;

    // בנאי פרטי
    private DalXml() {
        Initialization.Do(this);
    }
}
