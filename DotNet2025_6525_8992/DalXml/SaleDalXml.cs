using DO;
using DalApi;
using Dal;
namespace Dal;

internal class SaleDalXML : ISale
{
    
    readonly string s_path = XMLTools.GetFullPath("sales.xml");
    public int Create(Sale item)
    {
        List<Sale> list = XMLTools.LoadListFromXMLSerializer<Sale>(s_path);
        int nextId = Config.SaleId;

        Sale newItem = item with { Id = nextId }; // יצירת עותק עם ה-ID החדש
        list.Add(newItem);

        XMLTools.SaveListToXMLSerializer(list, s_path);
        return nextId;
    }

    public Sale? Read(int id) =>
        XMLTools.LoadListFromXMLSerializer<Sale>(s_path).FirstOrDefault(s => s.Id == id);

    public Sale? Read(Func<Sale, bool> filter) =>
        XMLTools.LoadListFromXMLSerializer<Sale>(s_path).FirstOrDefault(filter);

    public List<Sale> ReadAll(Func<Sale, bool>? filter = null)
    {
        List<Sale> list = XMLTools.LoadListFromXMLSerializer<Sale>(s_path);
        return filter == null ? list : list.Where(filter).ToList();
    }

    public void Update(Sale item)
    {
        List<Sale> list = XMLTools.LoadListFromXMLSerializer<Sale>(s_path);
        int index = list.FindIndex(s => s.Id == item.Id);

        if (index == -1) throw new Exception("Sale not found");

        list[index] = item;
        XMLTools.SaveListToXMLSerializer(list, s_path);
    }

    public void Delete(int id)
    {
        List<Sale> list = XMLTools.LoadListFromXMLSerializer<Sale>(s_path);
        Sale? item = list.FirstOrDefault(s => s.Id == id);

        if (item == null) throw new Exception("Sale not found");

        list.Remove(item);
        XMLTools.SaveListToXMLSerializer(list, s_path);
    }
}