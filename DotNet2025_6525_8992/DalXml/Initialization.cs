using DO;
using DalApi;

namespace Dal;

public static class Initialization
{
    private static IDal? s_dal;

    /// <summary>
    /// פונקציית האתחול הראשית - נקראת מהבנאי של DalXml
    /// </summary>
    /// <param name="dal">המופע של ה-DAL שדרכו נשמור את הנתונים</param>
    public static void Do(IDal dal)
    {
        s_dal = dal;

        try
        {
            // אם הלקוחות לא קיימים - צור אותם
            if (!s_dal.Customer.ReadAll().Any())
                CreateCustomers();

            // אם המוצרים לא קיימים - צור אותם
            if (!s_dal.Product.ReadAll().Any())
                CreateProducts();

            // אם המכירות לא קיימות - צור אותן
            if (!s_dal.Sale.ReadAll().Any())
                CreateSales();
        }
        catch (Exception ex)
        {
            // זריקת השגיאה למעלה - ה-UI יתפוס אותה ויציג אותה
            throw new Exception($"Initialization failed: {ex.Message}", ex);
        }
    }

    private static void CreateCustomers()
    {
        // הוספת לקוח מזדמן עם פרטים כלליים
        s_dal!.Customer.Create(new Customer
        {
            Id = 0, // או 1, תלוי איך ה-DAL שלך מייצר מזהים אוטומטיים
            CustomerName = "לקוח מזדמן",
            Address = "None",
            PhoneNumber = "000-0000000",
            IsClubMember = false
        });

        string[] names = { "Jonathan Veig", "Naama Veig", "Leah Reiner", "Yossi Cohen", "Itamar Levi" };
        string[] cities = { "New York", "New Jersey", "Jerusalem", "Tel Aviv", "Baltimore" };

        for (int i = 0; i < names.Length; i++)
        {
            s_dal!.Customer.Create(new Customer
            {
                CustomerName = names[i],
                Address = cities[i],
                PhoneNumber = $"050-412300{i}",
                IsClubMember = i % 2 == 0
            });
        }
    }

    private static void CreateProducts()
    {
        // רשימות שמות החיות (מקוצרות לצורך הדוגמה, השאירי את כל הרשימה המקורית שלך בקוד)
        string[] dogNames = { "Bulldog", "Golden Retriever", "Labrador", "Poodle", "Beagle", "Rottweiler", "German Shepherd", "Boxer", "Dachshund", "Husky", "Doberman", "Chihuahua", "Pomeranian", "Shih Tzu", "Cocker Spaniel", "Great Dane", "Akita", "Maltese", "Border Collie", "Pitbull" };
        string[] catNames = { "Siberian", "Persian", "Maine Coon", "British Shorthair", "Bengal", "Ragdoll", "Sphynx", "Scottish Fold", "Abyssinian", "Birman", "Russian Blue", "Norwegian Forest", "Oriental", "Himalayan", "Savannah", "Balinese", "Tonkinese", "Manx", "Chartreux", "Cornish Rex" };
        string[] fishNames = { "Goldfish", "Guppy", "Betta", "Angelfish", "Molly", "Platy", "Tetra", "Discus", "Oscar", "Corydoras", "Neon Tetra", "Zebrafish", "Swordtail", "Koi", "Arowana", "Pufferfish", "Rainbowfish", "Clownfish", "Blue Tang", "Lionfish" };
        string[] parrotNames = { "African Grey", "Macaw", "Cockatiel", "Budgerigar", "Amazon Parrot", "Lovebird", "Eclectus", "Conure", "Quaker Parrot", "Pionus", "Senegal Parrot", "Caique", "Rosella", "Lorikeet", "Ringneck" };
        string[] rabbitNames = { "Holland Lop", "Netherland Dwarf", "Lionhead", "Flemish Giant", "Mini Rex", "Dutch Rabbit", "English Lop", "French Lop", "Harlequin", "Rex", "Californian", "Polish", "Silver Fox", "Chinchilla Rabbit", "Havana" };
        string[] hamsterNames = { "Syrian Hamster", "Dwarf Hamster", "Roborovski", "Chinese Hamster", "Campbell Hamster", "Winter White", "Golden Hamster", "Black Bear", "Albino Hamster", "Long Haired Hamster" };

        // DOGS
        foreach (var name in dogNames)
            s_dal!.Product.Create(new Product { Name = name, Category = Categories.DOGS, Price = 500, Quantity = 20 });

        // CATS
        foreach (var name in catNames)
            s_dal!.Product.Create(new Product { Name = name, Category = Categories.CATS, Price = 350, Quantity = 15 });

        // FISH
        foreach (var name in fishNames)
            s_dal!.Product.Create(new Product { Name = name, Category = Categories.FISH, Price = 80, Quantity = 50 });

        // PARROTS
        foreach (var name in parrotNames)
            s_dal!.Product.Create(new Product { Name = name, Category = Categories.PARROTS, Price = 700, Quantity = 10 });

        // RABBITS
        foreach (var name in rabbitNames)
            s_dal!.Product.Create(new Product { Name = name, Category = Categories.RABBITS, Price = 300, Quantity = 12 });

        // HAMSTERS (תיקון שגיאת כתיב מ-HUMSTERS ל-HAMSTERS אם קיים ב-Enum)
        foreach (var name in hamsterNames)
            s_dal!.Product.Create(new Product { Name = name, Category = Categories.HUMSTERS, Price = 120, Quantity = 25 });
    }

    private static void CreateSales()
    {
        var productsList = s_dal!.Product.ReadAll().ToList();

        for (int i = 0; i < productsList.Count; i++)
        {
            s_dal.Sale.Create(new Sale
            {
                ProductId = productsList[i]!.Id,
                RequiredQuantity = i + 1,
                DiscountedPrice = productsList[i]!.Price * 0.8,
                IsForClubMembers = i % 2 == 0,
                SaleStartDate = DateTime.Now,
                SaleEndDate = DateTime.Now.AddMonths(1)
            });
        }
    }
    //private static void CreateOrders()
    //{
    //    s_dal!.Order.Create(new Order
    //    {
    //        Id = 1001,
    //        CustomerId = 1002,
    //        OrderDate = DateTime.Now,
    //        TotalPrice = 500,
    //        Items = new List<OrderItem>
    //{
    //    new OrderItem { OrderId = 1001, ProductId = 1005, Quantity = 1, PricePerUnit = 500 }
    //}
    //    });
    //}
}