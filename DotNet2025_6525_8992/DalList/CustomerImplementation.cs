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
        return DataSource.Customers.FirstOrDefault(c => c?.Id == id);
    }
    // קריאה של מוצר לפי פילטר
    public Customer? Read(Func<Customer, bool> filter)
    {
        return DataSource.Customers.FirstOrDefault(c => c != null && filter(c));
    }
    public List<Customer?> ReadAll(Func<Customer, bool>? filter = null) { 
        if (filter == null)
        {
            return DataSource.Customers.Select(c => c == null ? null : new Customer
            {
                Id = c.Id,
                CustomerName = c.CustomerName,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address
            }).ToList();
        }

        return DataSource.Customers
            .Where(c => c != null && filter(c)) 
            .Select(c => c == null ? null : new Customer
            {
                Id = c.Id,
                CustomerName = c.CustomerName,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address
            }).ToList();
    }
    public void Update(Customer item)
    {
        var oldItem = DataSource.Customers.FirstOrDefault(p => p.Id == item.Id);
        if (oldItem == null)
            throw new IdNotFoundException(item.Id, "Customer");

        int index = DataSource.Customers.IndexOf(oldItem);
        DataSource.Customers[index] = item;
    }

    public void Delete(int id)
    {
        if (!DataSource.Customers.Any(c => c.Id == id))
        {
            throw new IdNotFoundException(id, "Customer");
        }
        DataSource.Customers = DataSource.Customers
                                    .Where(p => p.Id != id)
                                    .ToList();
    }

   
}