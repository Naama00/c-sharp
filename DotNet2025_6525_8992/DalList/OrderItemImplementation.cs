using DO;
using DalApi;
using static Dal.DataSource;
using tools;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Dal;

internal class OrderItemImplementation : IOrderItem
{
    // יצירת פריט הזמנה חדש
    public int Create(OrderItem item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Attempting to add product {item.ProductId} to order {item.OrderId}");

        // בדרך כלל פריט הזמנה מקבל את ה-ID שלו מה-Config (מספר רץ)
        // אם ב-DO.OrderItem יש שדה Id, נעדכן אותו כאן
        OrderItem finalizedItem = item with { /* Id = Config.OrderItemId */ };
        DataSource.OrderItems.Add(finalizedItem);

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, "OrderItem created and added to DataSource.");

        return finalizedItem.OrderId;
    }

    // קריאת פריט לפי מזהה
    public OrderItem? Read(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Reading OrderItem with ID: {id}");

        return DataSource.OrderItems.FirstOrDefault(oi => oi?.OrderId == id);
    }

    // קריאה לפי פילטר (פונקציית תנאי)
    public OrderItem? Read(Func<OrderItem, bool> filter)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, "Reading OrderItem using filter.");

        return DataSource.OrderItems.FirstOrDefault(oi => oi != null && filter(oi));
    }

    // קריאת כל פריטי ההזמנה (עם או בלי פילטר)
    public List<OrderItem?> ReadAll(Func<OrderItem, bool>? filter = null)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, filter == null ? "Reading all OrderItems." : "Reading OrderItems with filter.");

        return DataSource.OrderItems
            .Where(oi => oi != null && (filter == null || filter(oi)))
            .Select(oi => oi == null ? null : new OrderItem
            {
                OrderId = oi.OrderId,
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                PricePerUnit = oi.PricePerUnit
            }).ToList();
    }

    // עדכון פריט קיים
    public void Update(OrderItem item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Updating OrderItem for Order {item.OrderId}, Product {item.ProductId}");

        var oldItem = DataSource.OrderItems.FirstOrDefault(oi => oi?.OrderId == item.OrderId && oi?.ProductId == item.ProductId);

        if (oldItem == null)
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, "Error: OrderItem not found for update.");
            throw new IdNotFoundException(item.OrderId, "OrderItem");
        }

        int index = DataSource.OrderItems.IndexOf(oldItem);
        DataSource.OrderItems[index] = item;
    }

    // מחיקת פריט/ים לפי מזהה הזמנה
    public void Delete(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"Deleting all items for Order ID: {id}");

        int removedCount = DataSource.OrderItems.RemoveAll(oi => oi?.OrderId == id);

        if (removedCount == 0)
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, "Warning: No items were found to delete.");
        }
    }

    // מתודות ספציפיות מהממשק IOrderItem
    public IEnumerable<OrderItem> ReadAllByOrder(int orderId)
    {
        return DataSource.OrderItems.Where(oi => oi != null && oi.OrderId == orderId)!;
    }

    public OrderItem ReadByProductAndOrder(int orderId, int productId)
    {
        return DataSource.OrderItems.FirstOrDefault(oi => oi != null && oi.OrderId == orderId && oi.ProductId == productId)!;
    }
}