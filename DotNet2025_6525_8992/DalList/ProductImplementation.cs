using DalApi;
using DO;
using static Dal.DataSource;

namespace Dal;

internal class ProductImplementation : IProduct
{
    public int Create(Product item)
    {
        Product finalizedItem = item with { Id = Config.ProductId };
        DataSource.Products.Add(finalizedItem);
        return finalizedItem.Id;
    }

    public Product? Read(int id)
    {
        Product item =DataSource.Products.Find(p => p?.Id == id);
        return item;
    }

    public List<Product?> ReadAll()
    {
            return DataSource.Products.Select(p => p == null ? null : new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price,
                    Quantity = p.Quantity
                })
                .ToList();
        }

    public void Update(Product item)
    {
        var itemIndex = from product in Products
                        where product.id = item.id
                        select item.id;

        if (!itemIndex.any())
            throw new IdNotFoundExcption(item.Id, "product");
        DataSource.Products[itemIndex] = item;
    }

    public void Delete(int id)
    {
        int itemIndex= DataSource.Products.FindIndex(p => p?.Id == id);
        if (itemIndex == -1)
            throw new IdNotFoundExcption(id, "product");
        DataSource.Products.RemoveAt(itemIndex);
    }
  
}