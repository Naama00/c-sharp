

namespace DO
{
    public record Product(int Id, string Name, CATEGORIES Category, double Price, int Quantity)
    {
        public Product() : this(0, string.Empty, default, 0.0, 0)
        {
        }
    }
}