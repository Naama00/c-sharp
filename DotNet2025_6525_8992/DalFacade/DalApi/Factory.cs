
namespace DalApi;
using static DalApi.DalConfig;
using System.Reflection;

public static class Factory
{
    public static IDal Get
    {
        get
        {
            // 1. ניסיון לקרוא מהקונפיגורציה
            string dalType = s_dalName ?? "xml"; // אם null, ברירת מחדל היא xml

            // 2. הגנה: אם המילון לא מכיל את המפתח, נוסיף אותו ידנית
            if (!s_dalPackages.ContainsKey(dalType))
            {
                s_dalPackages[dalType] = "DalXml";
            }

            string dal = s_dalPackages[dalType];

            try
            {
                // טעינת השכבה המתאימה
                return Assembly.Load(dal ?? throw new DalConfigException($"Package for {dalType} is null"))
                       .GetType($"Dal.{dal}")?.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?
                       .GetValue(null) as IDal
                       ?? throw new DalConfigException($"Class Dal.{dal} not found");
            }
            catch (Exception ex)
            {
                throw new DalConfigException($"Failed to load DAL instance", ex);
            }
        }
    }
}
