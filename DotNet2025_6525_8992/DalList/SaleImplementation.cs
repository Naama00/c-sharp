using DalApi;
using DO;
using static Dal.DataSource;
using tools; // ייבוא מחלקת הלוגים
using System.Reflection; // דרוש עבור MethodBase

namespace Dal;

internal class SaleImplementation : ISale
{
    public int Create(Sale item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Attempting to create sale for product ID: {item.ProductId}");

        Sale finalizedItem = item with { Id = Config.SaleId };
        DataSource.Sales.Add(finalizedItem);

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Sale created successfully. New ID: {finalizedItem.Id}");

        return finalizedItem.Id;
    }

    public Sale? Read(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Reading sale with ID: {id}");

        Sale? item = DataSource.Sales.Find(p => p?.Id == id);

        if (item == null)
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Sale with ID {id} not found.");

        return item;
    }

    // קריאה של מוצר לפי פילטר
    public Sale? Read(Func<Sale, bool> filter)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, "Reading sale using filter.");

        return DataSource.Sales.FirstOrDefault(s => s != null && filter(s));
    }

    public List<Sale?> ReadAll(Func<Sale, bool>? filter = null)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, filter == null ? "Reading all sales." : "Reading sales with filter.");

        // אם לא נשלח פילטר, נחזיר את כל הרשימה עם מיפוי לאובייקטים חדשים
        if (filter == null)
        {
            return DataSource.Sales.Select(p => p == null ? null : new Sale
            {
                Id = p.Id,
                ProductId = p.ProductId,
                RequiredQuantity = p.RequiredQuantity,
                DiscountedPrice = p.DiscountedPrice,
                IsForClubMembers = p.IsForClubMembers,
                SaleStartDate = p.SaleStartDate,
                SaleEndDate = p.SaleEndDate
            }).ToList();
        }

        // אם נשלח פילטר, נבצע קודם את הסינון ורק אז את המיפוי
        return DataSource.Sales
            .Where(p => p != null && filter(p)) // הסינון מתבצע כאן
            .Select(p => p == null ? null : new Sale
            {
                Id = p.Id,
                ProductId = p.ProductId,
                RequiredQuantity = p.RequiredQuantity,
                DiscountedPrice = p.DiscountedPrice,
                IsForClubMembers = p.IsForClubMembers,
                SaleStartDate = p.SaleStartDate,
                SaleEndDate = p.SaleEndDate
            }).ToList();
    }

    public void Update(Sale item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Updating sale with ID: {item.Id}");

        int itemIndex = DataSource.Sales.FindIndex(p => p?.Id == item.Id);
        if (itemIndex == -1)
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Error: Sale with ID {item.Id} not found for update.");
            throw new IdNotFoundException(item.Id, "sale");
        }

        DataSource.Sales[itemIndex] = item;
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Sale with ID {item.Id} updated successfully.");
    }

    public void Delete(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Attempting to delete sale with ID: {id}");

        int itemIndex = DataSource.Sales.FindIndex(p => p?.Id == id);
        if (itemIndex == -1)
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Error: Sale with ID {id} not found for deletion.");
            throw new IdNotFoundException(id, "sale");
        }

        DataSource.Sales.RemoveAt(itemIndex);
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Sale with ID {id} deleted successfully.");
    }
}