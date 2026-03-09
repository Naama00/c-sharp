using DalApi;
using DO;
using static Dal.DataSource;
using System.Linq;
using tools; // ייבוא מחלקת הלוגים
using System.Reflection; // דרוש עבור MethodBase

namespace Dal;

internal class ProductImplementation : IProduct
{
    public int Create(Product item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Attempting to create product: {item.Name}");

        Product finalizedItem = item with { Id = Config.ProductId };
        DataSource.Products.Add(finalizedItem);

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Product created successfully. New ID: {finalizedItem.Id}");

        return finalizedItem.Id;
    }

    public Product? Read(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Reading product with ID: {id}");

        var product = DataSource.Products.FirstOrDefault(p => p?.Id == id);

        if (product == null)
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Product with ID {id} not found.");

        return product;
    }

    // קריאה של מוצר לפי פילטר
    public Product? Read(Func<Product, bool> filter)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, "Reading product using filter.");

        return DataSource.Products.FirstOrDefault(p => p != null && filter(p));
    }

    public List<Product?> ReadAll(Func<Product, bool>? filter = null)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, filter == null ? "Reading all products." : "Reading products with filter.");

        //אם הפילטר ריק תחזיר את כל המוצרים
        if (filter == null)
            return DataSource.Products.Select(p => p == null ? null : new Product
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category,
                Price = p.Price,
                Quantity = p.Quantity
            })
            .ToList();

        return DataSource.Products
            //סינון המוצרים לפי הפילטר
            .Where(p => p != null && filter(p))
            .Select(p => p == null ? null : new Product
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
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Updating product with ID: {item.Id}");

        var oldItem = DataSource.Products.FirstOrDefault(p => p.Id == item.Id);
        if (oldItem == null)
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Error: Product with ID {item.Id} not found for update.");
            throw new IdNotFoundException(item.Id, "Product");
        }

        int index = DataSource.Products.IndexOf(oldItem);
        DataSource.Products[index] = item;

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Product with ID {item.Id} updated successfully.");
    }

    public void Delete(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Attempting to delete product with ID: {id}");

        if (!DataSource.Products.Any(p => p.Id == id))
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Error: Product with ID {id} not found for deletion.");
            throw new IdNotFoundException(id, "Product");
        }

        DataSource.Products = DataSource.Products
                                        .Where(p => p.Id != id)
                                        .ToList();

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Product with ID {id} deleted successfully.");
    }
}