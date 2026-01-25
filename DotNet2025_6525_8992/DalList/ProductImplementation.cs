using DalApi;
using DO;
using static Dal.DataSource;
using System.Linq;

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
        return DataSource.Products.FirstOrDefault(p => p?.Id == id);
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
        var oldItem = DataSource.Products.FirstOrDefault(p => p.Id == item.Id);
        if (oldItem == null)
            throw new IdNotFoundException(item.Id, "Product");

        int index = DataSource.Products.IndexOf(oldItem);
        DataSource.Products[index] = item;
    }
    public void Delete(int id)
    {
        if (!DataSource.Products.Any(p => p.Id == id))
        {
            throw new IdNotFoundException(id, "Product");
        }
        DataSource.Products = DataSource.Products
                                    .Where(p => p.Id != id)
                                    .ToList();
    }

}