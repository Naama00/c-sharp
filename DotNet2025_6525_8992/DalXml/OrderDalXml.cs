using DO;
using DalApi;
using System.Xml.Linq;
using tools;
using System.Reflection;
using Dal;
namespace Dal;

internal class OrderDalXml : IOrder
{
    // שם הקובץ כפי שמופיע בתיקיית הנתונים
    readonly string s_orders_xml = "orders.xml";

    public int Create(Order item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"XML: Attempting to create order for customer: {item.CustomerId}");

        // 1. טעינת הרשימה הקיימת מהקובץ
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order>(s_orders_xml);

        // 2. קבלת מזהה רץ חדש (מתוך קובץ ה-Config של ה-XML)
        int nextId = Config.NextOrderId;
        Order finalizedItem = item with { Id = nextId };

        // 3. הוספה לרשימה ושמירה מחדש
        orders.Add(finalizedItem);
        XMLTools.SaveListToXMLSerializer(orders, s_orders_xml);

        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"XML: Order {nextId} created successfully.");

        return nextId;
    }

    public Order? Read(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"XML: Reading order {id}");

        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order>(s_orders_xml);
        return orders.FirstOrDefault(o => o?.Id == id);
    }

    public Order? Read(Func<Order, bool> filter)
    {
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order>(s_orders_xml);
        return orders.FirstOrDefault(o => o != null && filter(o));
    }

    public List<Order?> ReadAll(Func<Order, bool>? filter = null)
    {
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order>(s_orders_xml);

        if (filter == null)
            return orders;

        return orders.Where(o => o != null && filter(o)).ToList();
    }

    public void Update(Order item)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"XML: Updating order {item.Id}");

        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order>(s_orders_xml);

        // מציאת האינדקס של האובייקט הישן
        int index = orders.FindIndex(o => o?.Id == item.Id);

        if (index == -1)
        {
            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                MethodBase.GetCurrentMethod().Name, $"XML Error: Order {item.Id} not found for update.");
            throw new IdNotFoundException(item.Id, "Order");
        }

        orders[index] = item;
        XMLTools.SaveListToXMLSerializer(orders, s_orders_xml);
    }

    public void Delete(int id)
    {
        LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
            MethodBase.GetCurrentMethod().Name, $"XML: Deleting order {id}");

        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order>(s_orders_xml);

        if (orders.RemoveAll(o => o?.Id == id) == 0)
        {
            throw new IdNotFoundException(id, "Order");
        }

        XMLTools.SaveListToXMLSerializer(orders, s_orders_xml);
    }
}