using DO;
using DalApi;
using Dal;

namespace DalTest;

public class Initialization
{

    private static ISale? sale;
    private static ICustomer? customer;
    private static IProduct? product;

    public Initialization()
    {
    }

    private static void CreateSales()
    {
        DataSource.Sales.Add(new DO.Sale
        {
            ID = DataSource.SaleId,
            CustomerID = 1,
            ProductID = 1,
            Quantity = 2,
            Date = DateTime.Now
        });
    }






}
