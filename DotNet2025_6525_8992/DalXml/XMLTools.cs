using System.Xml.Serialization;
using System.IO;

namespace Dal;

internal static class XMLTools
{
    // הנתיב המדויק בתוך תיקיית הריצה (bin)
    private static readonly string s_dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xml");

    static XMLTools()
    {
        // יצירת התיקייה בתוך ה-bin אם היא לא קיימת
        if (!Directory.Exists(s_dir))
        {
            Directory.CreateDirectory(s_dir);
        }
    }

    // פונקציית עזר לקבלת נתיב מלא - תשתמשי בה גם ב-ProductDalXml
    public static string GetFullPath(string fileName)
    {
        string cleanName = fileName.EndsWith(".xml") ? fileName : fileName + ".xml";
        return Path.Combine(s_dir, cleanName);
    }

    // שמירת רשימה לקובץ XML
    public static void SaveListToXMLSerializer<T>(List<T?> list, string filePath) where T : class
    {
        // שימוש בנתיב המאוחד!
        string fullPath = GetFullPath(filePath);

        try
        {
            using FileStream file = new(fullPath, FileMode.Create, FileAccess.Write);
            XmlSerializer serializer = new(typeof(List<T?>));
            serializer.Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create file at {fullPath}", ex);
        }
    }

    // טעינת רשימה מקובץ XML
    public static List<T?> LoadListFromXMLSerializer<T>(string filePath) where T : class
    {
        // שימוש באותו נתיב מאוחד!
        string fullPath = GetFullPath(filePath);

        try
        {
            if (!File.Exists(fullPath)) return new List<T?>();

            using FileStream file = new(fullPath, FileMode.Open, FileAccess.Read);
            XmlSerializer serializer = new(typeof(List<T?>));
            return (List<T?>)serializer.Deserialize(file)!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load XML file: {fullPath}", ex);
        }
    }
}