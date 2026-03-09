using DO;
using DalApi;
using static Dal.DataSource;
using tools;
using System.Reflection;

namespace Dal;
internal class CustomerImplementation : ICustomer
{
    public int Create(Customer item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Attempting to create customer: {item.CustomerName}");

        Customer finalizedItem = item with { Id = Config.CustomerId };
        DataSource.Customers.Add(finalizedItem);

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Customer created successfully. New ID: {finalizedItem.Id}");

        return finalizedItem.Id;
    }

    public Customer? Read(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Reading customer with ID: {id}");

        var customer = DataSource.Customers.FirstOrDefault(c => c?.Id == id);

        if (customer == null)
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Customer with ID {id} not found.");

        return customer;
    }

    // קריאה של מוצר לפי פילטר
    public Customer? Read(Func<Customer, bool> filter)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, "Reading customer using filter.");

        return DataSource.Customers.FirstOrDefault(c => c != null && filter(c));
    }

    public List<Customer?> ReadAll(Func<Customer, bool>? filter = null)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, filter == null ? "Reading all customers." : "Reading customers with filter.");

        //אם הפילטר ריק תחזיר את כל המוצרים
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
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Updating customer with ID: {item.Id}");

        var oldItem = DataSource.Customers.FirstOrDefault(p => p.Id == item.Id);
        if (oldItem == null)
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Error: Customer with ID {item.Id} not found for update.");
            throw new IdNotFoundException(item.Id, "Customer");
        }

        int index = DataSource.Customers.IndexOf(oldItem);
        DataSource.Customers[index] = item;

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Customer with ID {item.Id} updated successfully.");
    }

    public void Delete(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Attempting to delete customer with ID: {id}");

        if (!DataSource.Customers.Any(c => c.Id == id))
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Error: Customer with ID {id} not found for deletion.");
            throw new IdNotFoundException(id, "Customer");
        }

        DataSource.Customers = DataSource.Customers
                                        .Where(p => p.Id != id)
                                        .ToList();

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Customer with ID {id} deleted successfully.");
    }
}