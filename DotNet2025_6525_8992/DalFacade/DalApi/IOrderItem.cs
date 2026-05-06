using DO;
using System.Collections.Generic;

namespace DalApi;
public interface IOrderItem : ICrud<OrderItem>
{
    IEnumerable<OrderItem> ReadAllByOrder(int orderId);

    OrderItem ReadByProductAndOrder(int orderId, int productId);
}