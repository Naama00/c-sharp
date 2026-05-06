using System.Xml.Serialization;
using System.IO;

namespace Dal;

internal static class XMLTools
{
    // הגדרת נתיב התיקייה שבה נשמרים הקבצים
    // המשתנה הזה היה חסר ולכן קיבלת שגיאה
    private static readonly string s_dir = @"..\xml\";

    static XMLTools()
    {
        // בדיקה אם התיקייה קיימת, אם לא - יצירה שלה
        if (!Directory.Exists(s_dir))
            Directory.CreateDirectory(s_dir);
    }

    // שמירת רשימה לקובץ XML
    public static void SaveListToXMLSerializer<T>(List<T?> list, string filePath) where T : class
    {
        try
        {
            // שימוש ב-s_dir לבניית הנתיב המלא
            using FileStream file = new($"{s_dir}{filePath}.xml", FileMode.Create, FileAccess.Write);
            XmlSerializer serializer = new(typeof(List<T?>));
            serializer.Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save XML file: {filePath}", ex);
        }
    }

    // טעינת רשימה מקובץ XML
    public static List<T?> LoadListFromXMLSerializer<T>(string filePath) where T : class
    {
        try
        {
            string path = $"{s_dir}{filePath}.xml";
            if (!File.Exists(path)) return new List<T?>();

            using FileStream file = new(path, FileMode.Open, FileAccess.Read);
            XmlSerializer serializer = new(typeof(List<T?>));
            return (List<T?>)serializer.Deserialize(file)!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load XML file: {filePath}", ex);
        }
    }
}