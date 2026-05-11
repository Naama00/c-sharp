using System.Xml.Linq;
using DalApi;
using DO;
using Dal;
namespace Dal;

internal class ProductDalXml : IProduct
{
    readonly string s_path = XMLTools.GetFullPath("products.xml");

    // עזר להמרת XElement לאובייקט DO.Product
    static Product CreateProductFromElement(XElement p) => new Product
    {
        Id = (int)p.Element("Id")!,
        Name = (string)p.Element("Name")!,
        Category = Enum.TryParse<Categories>((string)p.Element("Category")!, out var result) ? result : default,
        Price = (double)p.Element("Price")!,
        Quantity = (int)p.Element("Quantity")!
    };

    public int Create(Product item)
    {
        XElement root;
        if (!File.Exists(s_path))
        {
            root = new XElement("Products"); // שם השורש ב-XML שלך
        }
        else
        {
            root = XElement.Load(s_path);
        }
        int nextId = Config.ProductId;

        XElement p = new XElement("Product",
            new XElement("Id", nextId),
            new XElement("Name", item.Name),
            new XElement("Category", item.Category),
            new XElement("Price", item.Price),
            new XElement("Quantity", item.Quantity)
        );

        root.Add(p);
        root.Save(s_path);
        return nextId;
    }

    public Product? Read(int id) =>
        XElement.Load(s_path).Elements("Product")
        .Select(p => CreateProductFromElement(p))
        .FirstOrDefault(p => p.Id == id);

    public Product? Read(Func<Product, bool> filter) =>
        XElement.Load(s_path).Elements("Product")
        .Select(p => CreateProductFromElement(p))
        .FirstOrDefault(filter);

    public List<Product> ReadAll(Func<Product, bool>? filter = null)
    {
        if (!File.Exists(s_path)) return new List<Product>();

        var list = XElement.Load(s_path).Elements("Product")
                           .Select(p => CreateProductFromElement(p));

        return filter == null ? list.ToList() : list.Where(filter).ToList();
    }

    public void Update(Product item)
    {
        XElement root = XElement.Load(s_path);
        XElement? p = root.Elements("Product").FirstOrDefault(x => (int)x.Element("Id")! == item.Id);

        if (p == null) throw new Exception("Product not found");

        p.Element("Name")!.Value = item.Name;
        p.Element("Category")!.Value = item.Category.ToString();
        p.Element("Price")!.Value = item.Price.ToString();
        p.Element("Quantity")!.Value = item.Quantity.ToString();

        root.Save(s_path);
    }

    public void Delete(int id)
    {
        XElement root = XElement.Load(s_path);
        XElement? p = root.Elements("Product").FirstOrDefault(x => (int)x.Element("Id")! == id);

        if (p == null) throw new Exception("Product not found");

        p.Remove();
        root.Save(s_path);
    }
}