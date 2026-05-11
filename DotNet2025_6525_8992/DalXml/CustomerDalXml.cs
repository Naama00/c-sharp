using DO;
using DalApi;
using Dal;
namespace Dal;

internal class CustomerDalXML : ICustomer
{
    readonly string s_path = XMLTools.GetFullPath("customers.xml");

    public int Create(Customer item)
    {
        // 1. טעינת כל רשימת הלקוחות מהקובץ
        List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(s_path);

        // 2. קבלת מזהה רץ חדש מהקונפיגורציה
        int nextId = Config.CustomerId;

        // 3. יצירת האובייקט החדש עם ה-ID שנוצר
        // (הנחת עבודה: Customer הוא class או record עם בנאי מתאים)
        Customer newCustomer = item with { Id = nextId };

        customers.Add(newCustomer);

        // 4. שמירה חזרה לקובץ
        XMLTools.SaveListToXMLSerializer(customers, s_path);

        return nextId;
    }

    public Customer? Read(int id)
    {
        return XMLTools.LoadListFromXMLSerializer<Customer>(s_path)
                       .FirstOrDefault(c => c.Id == id);
    }

    public Customer? Read(Func<Customer, bool> filter)
    {
        return XMLTools.LoadListFromXMLSerializer<Customer>(s_path)
                       .FirstOrDefault(filter);
    }

    public List<Customer> ReadAll(Func<Customer, bool>? filter = null)
    {
        // 1. בדיקה אם הקובץ קיים - מונע את הקריסה שראינו בתמונה!
        if (!File.Exists(s_path))
        {
            return new List<Customer>();
        }

        // 2. טעינת הרשימה מה-XML
        List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(s_path);

        // 3. החזרת הרשימה (עם או בלי סינון)
        if (filter == null)
            return customers;

        return customers.Where(filter).ToList();
    }

    public void Update(Customer item)
    {
        List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(s_path);

        // מציאת האינדקס של הלקוח לעדכון
        int index = customers.FindIndex(c => c.Id == item.Id);

        if (index == -1)
            throw new Exception($"Customer with ID {item.Id} was not found.");

        // עדכון הרשימה ושמירה
        customers[index] = item;
        XMLTools.SaveListToXMLSerializer(customers, s_path);
    }

    public void Delete(int id)
    {
        List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(s_path);

        Customer? customer = customers.FirstOrDefault(c => c.Id == id);
        if (customer == null)
            throw new Exception($"Customer with ID {id} does not exist.");

        customers.Remove(customer);
        XMLTools.SaveListToXMLSerializer(customers, s_path);
    }
}