using DO;
using DalApi;
using static Dal.DataSource;

namespace Dal;
internal class CustomerImplementation : ICustomer
{
    public int Create(Customer item)
    {
        Customer finalizedItem = item with { Id = Config.CustomerId };
        DataSource.Customers.Add(finalizedItem);
        return finalizedItem.Id;
    }

    public Customer? Read(int id)
    {
        Customer item= DataSource.Customers.Find(p => p?.Id == id);
        return item;
    }

    public List<Customer?> ReadAll()
    {
        return DataSource.Customers.Select(p => p == null ? null : new Customer
        {
            Id = p.Id,
            CustomerName = p.CustomerName,
            PhoneNumber = p.PhoneNumber,
            Address = p.Address
        }).ToList();
    }

    public void Update(Customer item)
    {
        var itemIndex= DataSource.Customers.Where(p => p?.Id == item.Id);
        if (itemIndex == -1)
            throw new IdNotFoundExcption(item.Id,"customer");
        DataSource.Customers[itemIndex] = item;
    }

    public void Delete(int id)
    {
        var itemIndex= DataSource.Customers.Where(p => p?.Id == id);
        if (itemIndex == -1)
            throw new IdNotFoundExcption(id, "customer");
        DataSource.Customers.RemoveAt(itemIndex);
    }

   
}