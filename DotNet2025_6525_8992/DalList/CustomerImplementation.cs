using DO;
using DalApi;


namespace Dal;

internal class CustomerImplementation : ICustomer
{
    public int Create(Customer item)
    {

        DataSource.Customers.Add(item);
        return item.Id;
    }

    public Customer? Read(int id)
    {
        Customer item= DataSource.Customers.Find(p => p?.Id == id);
        return item;
    }

    public List<Customer?> ReadAll()
    {
        return DataSource.Customers;
    }

    public void Update(Customer item)
    {
        int itemIndex= DataSource.Customers.FindIndex(p => p?.Id == item.Id);
        if (itemIndex == -1)
            throw new KeyNotFoundException($"Customer with Id {item.Id} not found.");
        DataSource.Customers[itemIndex] = item;
    }

    public void Delete(int id)
    {
        int itemIndex= DataSource.Customers.FindIndex(p => p?.Id == id);
        if (itemIndex == -1)
            throw new KeyNotFoundException($"Customer with Id {id} not found.");
        DataSource.Customers.RemoveAt(itemIndex);
    }

    public void PrintAll()
    {
        foreach(var customer in DataSource.Customers)
        {
            Console.WriteLine(customer?.Id+" "+customer?.CustomerName+" "+customer?.PhoneNumber+" "+customer?.Address);
        }
    }
}