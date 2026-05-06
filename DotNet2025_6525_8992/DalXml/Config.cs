using System.Xml.Linq;

namespace Dal;

internal static class Config
{
    private static string s_config_xml = "data-config";
    private static string s_path = $@"xml\{s_config_xml}.xml";

    // תכונה עבור מספר מוצר רץ
    internal static int ProductId
    {
        get => GetAndIncrement("NextProductId");
    }

    // תכונה עבור מספר לקוח רץ
    internal static int CustomerId
    {
        get => GetAndIncrement("NextCustomerId");
    }

    // תכונה עבור מספר מכירה רץ
    internal static int SaleId
    {
        get => GetAndIncrement("NextSaleId");
    }
    // תכונה עבור מספר הזמנה רץ
    internal static int NextOrderId
    {
        get => GetAndIncrement("NextOrderId");
    }
    /// <summary>
    /// פונקציה שמבצעת את פעולת ה-get המבוקשת: 
    /// קריאה מהקובץ, קידום המספר, שמירה והחזרת הערך.
    /// </summary>
    private static int GetAndIncrement(string elementName)
    {
        if (!File.Exists(s_path))
            throw new Exception($"קובץ הקונפיגורציה לא נמצא בנתיב: {Path.GetFullPath(s_path)}");

        try
        {
            XElement root = XElement.Load(s_path);
            XElement? node = root.Element(elementName);

            if (node == null)
                throw new Exception($"האלמנט {elementName} לא נמצא בקובץ הקונפיגורציה.");

            int currentVal = int.Parse(node.Value);
            int nextVal = currentVal + 1;

            // עדכון ושמירה לקובץ
            node.Value = nextVal.ToString();
            root.Save(s_path);

            return nextVal; // מחזירים את הערך המקודם (בדומה ל- ++NextId)
        }
        catch (Exception ex)
        {
            throw new Exception($"שגיאה בגישה לנתוני קונפיגורציה: {ex.Message}");
        }
    }
}