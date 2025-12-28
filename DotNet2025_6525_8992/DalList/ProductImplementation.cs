using DalApi;
using DO;

namespace Dal;

internal class ProductImplementation : IProduct
{
    public int Create(Product item)
    {

        DataSource.Products.Add(item);
        return item.Id;
    }

    public Product? Read(int id)
    {
        Product item =DataSource.Products.Find(p => p?.Id == id);
        return item;
    }

    public List<Product?> ReadAll()
    {
        return DataSource.Products;
    }

    public void Update(Product item)
    {
        int itemIndex=DataSource.Products.FindIndex(p => p?.Id == item.Id);
        if (itemIndex == -1)
            throw new IdNotFoundExcptions($"Product with Id {item.Id} not found.");
        DataSource.Products[itemIndex] = item;
    }

    public void Delete(int id)
    {
        int itemIndex= DataSource.Products.FindIndex(p => p?.Id == id);
        if (itemIndex == -1)
            throw new IdNotFoundExcptions($"Product with Id {id} not found.");
        DataSource.Products.RemoveAt(itemIndex);
    }
    public void PrintAll()
    {
        foreach (var product in DataSource.Products)
        {
            Console.WriteLine(product?.Id + " " + product?.Name + " " + product?.Category + " " + product?.Quantity + " " + product?.Price);
        }
    }
}