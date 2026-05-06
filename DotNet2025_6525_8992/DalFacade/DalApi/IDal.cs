using DO;

namespace DalApi;

public interface IDal
{
    IProduct Product { get; }
    ICustomer Customer { get; }
    ISale Sale { get; }
    IOrder Order { get; }
    IOrderItem OrderItem { get; }
}