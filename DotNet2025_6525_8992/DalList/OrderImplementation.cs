using DO;
using DalApi;
using static Dal.DataSource;
using tools;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Dal;

internal class OrderImplementation : IOrder
{
    public int Create(Order item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Attempting to create order for customer ID: {item.CustomerId}");

        // יצירת מזהה רץ אוטומטי להזמנה
        Order finalizedItem = item with { Id = Config.OrderId };
        DataSource.Orders.Add(finalizedItem);

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Order created successfully. New ID: {finalizedItem.Id}");

        return finalizedItem.Id;
    }

    public Order? Read(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Reading order with ID: {id}");

        var order = DataSource.Orders.FirstOrDefault(o => o?.Id == id);

        if (order == null)
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Order with ID {id} not found.");

        return order;
    }

    public Order? Read(Func<Order, bool> filter)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, "Reading order using filter.");

        return DataSource.Orders.FirstOrDefault(o => o != null && filter(o));
    }

    public List<Order?> ReadAll(Func<Order, bool>? filter = null)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, filter == null ? "Reading all orders." : "Reading orders with filter.");

        if (filter == null)
        {
            return DataSource.Orders.Select(o => o == null ? null : new Order
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice
            }).ToList();
        }

        return DataSource.Orders
            .Where(o => o != null && filter(o))
            .Select(o => o == null ? null : new Order
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice
            }).ToList();
    }

    public void Update(Order item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Updating order with ID: {item.Id}");

        var oldItem = DataSource.Orders.FirstOrDefault(o => o?.Id == item.Id);
        if (oldItem == null)
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Error: Order with ID {item.Id} not found for update.");
            throw new IdNotFoundException(item.Id, "Order");
        }

        int index = DataSource.Orders.IndexOf(oldItem);
        DataSource.Orders[index] = item;

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Order with ID {item.Id} updated successfully.");
    }

    public void Delete(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Attempting to delete order with ID: {id}");

        var order = DataSource.Orders.FirstOrDefault(o => o?.Id == id);
        if (order == null)
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"Error: Order with ID {id} not found for deletion.");
            throw new IdNotFoundException(id, "Order");
        }

        DataSource.Orders.Remove(order);

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Order with ID {id} deleted successfully.");
    }
}