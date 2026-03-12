using System.Xml.Serialization;

namespace Dal;

internal static class XMLTools
{
    public static void SaveListToXmlSerializer<T>(List<T> list, string filePath)
    {
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write);
            XmlSerializer serializer = new(typeof(List<T>));
            serializer.Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save XML to {filePath}", ex);
        }
    }

    public static List<T> LoadListFromXmlSerializer<T>(string filePath)
    {
        try
        {
            if (!File.Exists(filePath)) return new List<T>();
            using FileStream file = new(filePath, FileMode.Open, FileAccess.Read);
            XmlSerializer serializer = new(typeof(List<T>));
            return (List<T>)serializer.Deserialize(file)!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load XML from {filePath}", ex);
        }
    }
}