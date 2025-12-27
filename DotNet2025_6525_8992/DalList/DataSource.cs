
namespace Dal;


internal static class DataSource
{
    internal static List<DO.Product?> Products = new();
    internal static List<DO.Sale?> Sales = new();
    internal static List<DO.Customer?> Customers = new();

    internal class Config
    {
        internal const int minProductId = 100;
        internal const int minCustomerId = 100;
        internal const int minSaleId = 100;

        internal static int NextProductId = minProductId;
        internal static int NextCustomerId = minCustomerId;
        internal static int NextSaleId = minSaleId;

        internal static int ProductId => ++NextProductId;
        internal static int CustomerId => ++NextCustomerId;
        internal static int SaleId => ++NextSaleId;
    }
}


