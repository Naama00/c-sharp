
namespace Dal;


internal static class DataSource
{
    internal static List<DO.Product?> Products = new();
    internal static List<DO.Sale?> Sales = new();
    internal static List<DO.Customer?> Customers = new();
    internal static List<DO.Order?> Orders = new();
    internal static List<DO.OrderItem?> OrderItems = new();
    internal class Config
    {
        internal const int minProductId = 100;
        internal const int minCustomerId = 300000000;
        internal const int minSaleId = 100;
        internal const int minOrderId = 1000;
        internal const int minOrderItemId = 10000;

        internal static int NextProductId = minProductId;
        internal static int NextCustomerId = minCustomerId;
        internal static int NextSaleId = minSaleId;
        internal static int NextOrderId = minOrderId;
        

        internal static int ProductId => ++NextProductId;
        internal static int CustomerId => ++NextCustomerId;
        internal static int SaleId => ++NextSaleId;

        internal static int OrderId => ++NextOrderId;
    }
}


