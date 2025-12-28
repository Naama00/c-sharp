using DO;
using DalApi;
using Dal;

namespace DalTest;

public static class Initialization
{
    private static ISale? sale;
    private static ICustomer? customer;
    private static IProduct? product;

    public static void Initialize(ISale isale, ICustomer icustomer, IProduct iproduct)
    {
        sale = isale;
        customer = icustomer;
        product = iproduct;

        CreateCustomers();
        CreateProducts();
        CreateSales();
    }

    private static void CreateCustomers()
    {
        string[] names = { "Jonathan Veig", "Naama Veig","Leah Reiner", "Yossi Cohen", "Itamar Levi" };
        string[] cities = { "New York", "New Jersey", "Jerusalem", "Tel Aviv","Boltimore" };

        for (int i = 0; i < names.Length; i++)
        {
            customer?.Create(new()
            {
                Id = DataSource.Config.CustomerId,
                CustomerName = names[i],
                Address = cities[i],
                PhoneNumber = $"050-412300{i}"
            });
        }
    }

    private static void CreateProducts()
    {
        string[] dogNames = { "Buldog", "Golden Retriever", "Labrador" };
        string[] catNames = { "Sibirian cat", "Persian cat" };

        // יצירת כלבים
        foreach (var name in dogNames)
            product?.Create(new() { Id = DataSource.Config.ProductId, Name = name, Category = CATEGORIES.DOGS, Price = 500, Quantity = 20 });

        // יצירת חתולים
        foreach (var name in catNames)
            product?.Create(new() { Id = DataSource.Config.ProductId, Name = name, Category = CATEGORIES.CATS, Price = 350, Quantity = 15 });
    }

    private static void CreateSales()
    {
        var productsList = product?.ReadAll()?.ToList();

        for (int i = 0; i < productsList?.Count; i++)
        {

            sale?.Create(new()
            {
                Id = DataSource.Config.SaleId,
                ProductId = productsList[i].Id,
                RequiredQuantity = i + 1,
                DiscountedPrice = productsList[i].Price * 0.8,
                IsForClubMembers = i % 2 == 0,
                SaleStartDate = DateTime.Now,
                SaleEndDate = DateTime.Now.AddMonths(1)
            });
        }
    }
}