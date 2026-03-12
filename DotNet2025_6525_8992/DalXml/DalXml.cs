using DalApi;

namespace Dal;

internal sealed class DalXml : IDal
{
    // מימוש התכונות מהממשק IDal 
    public IProduct Product { get; } = new ProductDalXml();
    public ISale Sale { get; } = new SaleDalXml();
    public ICustomer Customer { get; } = new CustomerDalXml();

    // מימוש ה-Singleton שה-Factory שלך מצפה לו
    private static readonly DalXml instance = new DalXml();
    public static DalXml Instance => instance;

    // בנאי פרטי
    private DalXml() { }
}