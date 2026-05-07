using DO;
using DalApi;
using System.Reflection;
using tools;
using Dal;
namespace Dal;

internal class OrderItemDalXML : IOrderItem
{
    // שם הקובץ בתיקיית הנתונים
    readonly string s_orderItems_XML = "orderItems.xml";

    public int Create(OrderItem item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"XML: Adding product {item.ProductId} to order {item.OrderId}");

        List<OrderItem?> items = XMLTools.LoadListFromXMLSerializer<OrderItem>(s_orderItems_XML);

        // בדרך כלל ב-OrderItem משתמשים ב-ID רץ אם הוגדר כזה ב-DO
        // כאן נניח שה-ID מנוהל ב-Config
        int nextId = Config.NextOrderId;
        OrderItem finalizedItem = item with { OrderId = nextId };

        items.Add(finalizedItem);
        XMLTools.SaveListToXMLSerializer(items, s_orderItems_XML);

        return finalizedItem.OrderId;
    }

    public OrderItem? Read(int id)
    {
        List<OrderItem?> items = XMLTools.LoadListFromXMLSerializer<OrderItem>(s_orderItems_XML);
        return items.FirstOrDefault(oi => oi?.OrderId == id);
    }

    public OrderItem? Read(Func<OrderItem, bool> filter)
    {
        List<OrderItem?> items = XMLTools.LoadListFromXMLSerializer<OrderItem>(s_orderItems_XML);
        return items.FirstOrDefault(oi => oi != null && filter(oi));
    }

    public List<OrderItem?> ReadAll(Func<OrderItem, bool>? filter = null)
    {
        List<OrderItem?> items = XMLTools.LoadListFromXMLSerializer<OrderItem>(s_orderItems_XML);
        return filter == null ? items : items.Where(oi => oi != null && filter(oi)).ToList();
    }

    public void Update(OrderItem item)
    {
        List<OrderItem?> items = XMLTools.LoadListFromXMLSerializer<OrderItem>(s_orderItems_XML);

        int index = items.FindIndex(oi => oi?.OrderId == item.OrderId);
        if (index == -1)
            throw new IdNotFoundException(item.OrderId, "OrderItem");

        items[index] = item;
        XMLTools.SaveListToXMLSerializer(items, s_orderItems_XML);
    }

    public void Delete(int id)
    {
        List<OrderItem?> items = XMLTools.LoadListFromXMLSerializer<OrderItem>(s_orderItems_XML);

        if (items.RemoveAll(oi => oi?.OrderId == id) == 0)
            throw new IdNotFoundException(id, "OrderItem");

        XMLTools.SaveListToXMLSerializer(items, s_orderItems_XML);
    }

    // --- מימוש מתודות ספציפיות מ-IOrderItem ---

    public IEnumerable<OrderItem> ReadAllByOrder(int orderId)
    {
        // שליפת כל הפריטים של הזמנה מסוימת מהקובץ
        List<OrderItem?> items = XMLTools.LoadListFromXMLSerializer<OrderItem>(s_orderItems_XML);
        return items.Where(oi => oi != null && oi.OrderId == orderId).Cast<OrderItem>();
    }

    public OrderItem ReadByProductAndOrder(int orderId, int productId)
    {
        List<OrderItem?> items = XMLTools.LoadListFromXMLSerializer<OrderItem>(s_orderItems_XML);
        var item = items.FirstOrDefault(oi => oi != null && oi.OrderId == orderId && oi.ProductId == productId);
        return item ?? throw new IdNotFoundException(orderId, $"OrderItem (Product: {productId})");
    }
}