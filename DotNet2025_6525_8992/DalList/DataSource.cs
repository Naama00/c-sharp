namespace Dal;

internal static class DataSource
{
    internal static List<DO.Product?> Products = new();
    internal static List<DO.Sale?> Sales = new();
    internal static List<DO.Customer?> Customers = new();
    // ייתכן שיש כאן גם משתנה עזר לקידום רץ של מזהים
    internal class Config
    {
        internal const int minProductId = 100;
        internal const int minCustomerId = 100;
        internal const int minSaleId = 100;

        internal static int NextProductId = 100;
        internal static int NextCustomerId = 100;
        internal static int NextSaleId = 100;

        int ProductId => ++NextProductId;
        int CustomerId => ++NextCustomerId;
        int SaleId => ++NextSaleId;
    }
}


