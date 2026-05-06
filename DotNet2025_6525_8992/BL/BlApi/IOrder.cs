using BL.BO;
using System;
using System.Collections.Generic;

namespace BL.BlApi;

public interface IOrder
{
  
    int DoOrder(Order order);
    double CalculateItemPrice(int productId, int quantity, bool isClubMember);

    double GetTotalOrderSum(List<OrderItem> items, bool isClubMember);
    List<Order> ReadAllOrders(Func<Order, bool>? filter = null);

    Order? GetOrderDetails(int orderId);

    bool IsStockAvailable(List<OrderItem> items);
}