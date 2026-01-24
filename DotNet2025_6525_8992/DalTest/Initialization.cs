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
                CustomerName = names[i],
                Address = cities[i],
                PhoneNumber = $"050-412300{i}"
            });
        }
    }

    private static void CreateProducts()
    {
        string[] dogNames =
  {
    "Bulldog","Golden Retriever","Labrador","Poodle","Beagle",
    "Rottweiler","German Shepherd","Boxer","Dachshund","Husky",
    "Doberman","Chihuahua","Pomeranian","Shih Tzu","Cocker Spaniel",
    "Great Dane","Akita","Maltese","Border Collie","Pitbull"
};

        string[] catNames =
        {
    "Siberian","Persian","Maine Coon","British Shorthair","Bengal",
    "Ragdoll","Sphynx","Scottish Fold","Abyssinian","Birman",
    "Russian Blue","Norwegian Forest","Oriental","Himalayan",
    "Savannah","Balinese","Tonkinese","Manx","Chartreux","Cornish Rex"
};

        string[] fishNames =
        {
    "Goldfish","Guppy","Betta","Angelfish","Molly",
    "Platy","Tetra","Discus","Oscar","Corydoras",
    "Neon Tetra","Zebrafish","Swordtail","Koi","Arowana",
    "Pufferfish","Rainbowfish","Clownfish","Blue Tang","Lionfish"
};

        string[] parrotNames =
        {
    "African Grey","Macaw","Cockatiel","Budgerigar","Amazon Parrot",
    "Lovebird","Eclectus","Conure","Quaker Parrot","Pionus",
    "Senegal Parrot","Caique","Rosella","Lorikeet","Ringneck"
};

        string[] rabbitNames =
        {
    "Holland Lop","Netherland Dwarf","Lionhead","Flemish Giant","Mini Rex",
    "Dutch Rabbit","English Lop","French Lop","Harlequin","Rex",
    "Californian","Polish","Silver Fox","Chinchilla Rabbit","Havana"
};

        string[] hamsterNames =
        {
    "Syrian Hamster","Dwarf Hamster","Roborovski","Chinese Hamster",
    "Campbell Hamster","Winter White","Golden Hamster","Black Bear",
    "Albino Hamster","Long Haired Hamster"
};

        // DOGS
        foreach (var name in dogNames)
            s_dal.Product?.Create(new()
            {
                Name = name,
                Category = Categories.DOGS,
                Price = 500,
                Quantity = 20
            });

        // CATS
        foreach (var name in catNames)
            s_dal.Product?.Create(new()
            {
                Name = name,
                Category = Categories.CATS,
                Price = 350,
                Quantity = 15
            });

        // FISH
        foreach (var name in fishNames)
            s_dal.Product?.Create(new()
            {
                Id = DataSource.Config.ProductId,
                Name = name,
                Category = Categories.FISH,
                Price = 80,
                Quantity = 50
            });

        // PARROTS
        foreach (var name in parrotNames)
            s_dal.Product?.Create(new()
            {
                Name = name,
                Category = Categories.PARROTS,
                Price = 700,
                Quantity = 10
            });

        // RABBITS
        foreach (var name in rabbitNames)
            s_dal.Product?.Create(new()
            {
                Name = name,
                Category = Categories.RABBITS,
                Price = 300,
                Quantity = 12
            });

        // HUMSTERS
        foreach (var name in hamsterNames)
            s_dal.Product?.Create(new()
            {
                Name = name,
                Category = Categories.HUMSTERS,
                Price = 120,
                Quantity = 25
            });
        
    }
    
    private static void CreateSales()
    {
        var productsList = s_dal.Product?.ReadAll()?.ToList();

        for (int i = 0; i < productsList?.Count; i++)
        {

            s_dal.Sale?.Create(new()
            {
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