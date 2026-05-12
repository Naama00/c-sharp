using BL.BlApi;
using BL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.BlImplementation;

internal class OrderImplementation : IOrder
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int DoOrder(Order order)
    {
        // 1. בדיקות תקינות קלט (נשאר בדיוק אותו דבר)
        if (order == null) throw new BLNullPropertyException("Order", "Object");
        if (order.Items == null || !order.Items.Any())
            throw new BLInvalidInputException("Cannot place an empty order.");

        // 2. בדיקת מלאי וקיום מוצרים (נשאר אותו דבר)
        foreach (var item in order.Items)
        {
            var doProduct = _dal.Product.Read(item.ProductId);
            if (doProduct == null) throw new BLIdNotFoundException(item.ProductId, "Product");
            if (doProduct.Quantity < item.Quantity)
                throw new BLOutOfStockException(item.ProductId, doProduct.Name);
        }

        // 3. בדיקת קיום לקוח וחישוב מחירים (נשאר אותו דבר)
        var customer = _dal.Customer.Read(order.CustomerId);
        if (customer == null) throw new BLIdNotFoundException(order.CustomerId, "Customer");

        bool isClubMember = customer.IsClubMember;
        double totalOrderPrice = 0;

        foreach (var item in order.Items)
        {
            double itemTotalPrice = CalculateItemPrice(item.ProductId, item.Quantity, isClubMember);
            item.PricePerUnit = itemTotalPrice / item.Quantity;
            totalOrderPrice += itemTotalPrice;
        }

        order.TotalPrice = totalOrderPrice;
        order.OrderDate = DateTime.Now;

        // 4. תהליך השמירה החדש - כאן השינוי המרכזי
        try
        {
            // הפעולה הזו שומרת את ה-Order כולל רשימת ה-Items שלו בתוך קובץ ה-XML
            int newOrderId = _dal.Order.Create(order.ToDo());

            // עדכון המלאי בלבד (ללא קריאה ל-OrderItem.Create)
            foreach (var item in order.Items)
            {
                var doProduct = _dal.Product.Read(item.ProductId);
                if (doProduct != null)
                {
                    _dal.Product.Update(doProduct with { Quantity = doProduct.Quantity - item.Quantity });
                }
            }

            return newOrderId;
        }
        catch (Exception ex)
        {
            // הטיפול בחריגות נשאר אותו דבר
            throw new BLOrderProcessException("Failed to save order.", ex);
        }
    }
    public double CalculateItemPrice(int productId, int quantity, bool isClubMember)
    {
        var doProduct = _dal.Product.Read(productId);
        if (doProduct == null) return 0;

        double basePrice = doProduct.Price * quantity;

        // חיפוש מבצע פעיל
        var activeSale = (from s in _dal.Sale.ReadAll()
                          where s != null &&
                                s.ProductId == productId &&
                                s.SaleStartDate <= DateTime.Now &&
                                s.SaleEndDate >= DateTime.Now &&
                                (!s.IsForClubMembers || isClubMember) &&
                                quantity >= s.RequiredQuantity
                          select s).FirstOrDefault();

        return activeSale != null ? activeSale.DiscountedPrice * quantity : basePrice;
    }

    public double GetTotalOrderSum(List<OrderItem> items, bool isClubMember)
    {
        return items.Sum(item => CalculateItemPrice(item.ProductId, item.Quantity, isClubMember));
    }

    public bool IsStockAvailable(List<OrderItem> items)
    {
        return items.All(item => {
            var p = _dal.Product.Read(item.ProductId);
            return p != null && p.Quantity >= item.Quantity;
        });
    }
    public List<Order> ReadAllOrders(Func<Order, bool>? filter = null)
    {
        return (from doOrd in _dal.Order.ReadAll()
                let boOrd = doOrd.ToBo() 
                where filter == null || filter(boOrd)
                select boOrd).ToList();
    }
    public Order? GetOrderDetails(int orderId)
    {
        var doOrder = _dal.Order.Read(orderId);
        if (doOrder == null) return null;
        return doOrder.ToBo();
    }
}