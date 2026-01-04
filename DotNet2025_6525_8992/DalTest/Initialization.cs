using DO;
using DalApi;
using Dal;

namespace DalTest;

public static class Initialization
{
    private static IDal? s_dal;
    public static void Initialize(IDal dal)
    {
        s_dal = dal;
        CreateCustomers();
        CreateProducts();
        CreateSales();
    }

    private static void CreateCustomers()
    {
        string[] names = { "Jonathan Veig", "Naama Veig", "Leah Reiner", "Yossi Cohen", "Itamar Levi" };
        string[] cities = { "New York", "New Jersey", "Jerusalem", "Tel Aviv", "Boltimore" };

        for (int i = 0; i < names.Length; i++)
        {
            s_dal.Customer?.Create(new()
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
           s_dal.Product?.Create(new() { Id = DataSource.Config.ProductId, Name = name, Category = Categories.DOGS, Price = 500, Quantity = 20 });

        // יצירת חתולים
        foreach (var name in catNames)
            s_dal.Product?.Create(new() { Id = DataSource.Config.ProductId, Name = name, Category = Categories.CATS, Price = 350, Quantity = 15 });
    }

    private static void CreateSales()
    {
        var productsList = s_dal.Product?.ReadAll()?.ToList();

        for (int i = 0; i < productsList?.Count; i++)
        {

            s_dal.Sale?.Create(new()
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