using DalApi;
using DO;
using static Dal.DataSource;

namespace Dal;

internal class SaleImplementation : ISale
{
    public int Create(Sale item)
    {
        Sale finalizedItem = item with { Id = Config.SaleId };
        DataSource.Sales.Add(finalizedItem);
        return finalizedItem.Id;
    }

    public Sale? Read(int id)
    {
        Sale item = DataSource.Sales.Find(p => p?.Id == id);
        return item;
    }

    public List<Sale?> ReadAll()
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

    public void Update(Sale item)
    {
        int itemIndex = DataSource.Sales.FindIndex(p => p?.Id == item.Id);
        if (itemIndex == -1)
            throw new IdNotFoundExcption(item.Id, "sale");
        DataSource.Sales[itemIndex] = item;
    }

    public void Delete(int id)
    {
        int itemIndex = DataSource.Sales.FindIndex(p => p?.Id == id);
        if (itemIndex == -1)
            throw new IdNotFoundExcption(id, "sale");
        DataSource.Sales.RemoveAt(itemIndex);
    }

}