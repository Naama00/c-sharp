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
        // 1. בדיקות תקינות קלט בסיסיות (Validation)
        if (order == null)
            throw new BLNullPropertyException("Order", "Object");

        if (order.Items == null || !order.Items.Any())
            throw new BLInvalidInputException("Cannot place an empty order. Please add products to your cart.");

        // 2. בדיקת מלאי וקיום מוצרים
        foreach (var item in order.Items)
        {
            var doProduct = _dal.Product.Read(item.ProductId);

            if (doProduct == null)
                throw new BLIdNotFoundException(item.ProductId, "Product");

            if (doProduct.Quantity < item.Quantity)
            {
                throw new BLOutOfStockException(item.ProductId, doProduct.Name);
            }
        }

        // 3. בדיקת קיום לקוח וחישוב מחירים
        var customer = _dal.Customer.Read(order.CustomerId);
        if (customer == null)
        {
            throw new BLIdNotFoundException(order.CustomerId, "Customer");
        }

        bool isClubMember = customer.IsClubMember;
        double totalOrderPrice = 0;

        foreach (var item in order.Items)
        {
            // בתוך CalculateItemPrice כבר קיימת חריגה אם המוצר לא נמצא
            double itemTotalPrice = CalculateItemPrice(item.ProductId, item.Quantity, isClubMember);

            item.PricePerUnit = itemTotalPrice / item.Quantity;
            totalOrderPrice += itemTotalPrice;
        }

        if (totalOrderPrice <= 0)
            throw new BLDataValidationException("Order", "TotalPrice", "must be greater than zero");

        order.TotalPrice = totalOrderPrice;
        order.OrderDate = DateTime.Now;

        // 4. תהליך השמירה ב-DAL
        try
        {
            // שמירת ההזמנה הראשית
            int newOrderId = _dal.Order.Create(Tools.ToDo(order));

            // שמירת פריטי ההזמנה ועדכון המלאי
            foreach (var item in order.Items)
            {
                // שמירת פריט ההזמנה (מקושר ל-ID שנוצר)
                _dal.OrderItem.Create(item.ToDo(newOrderId));

                // עדכון המלאי ב-DAL
                var doProduct = _dal.Product.Read(item.ProductId);
                if (doProduct != null)
                {
                    _dal.Product.Update(doProduct with { Quantity = doProduct.Quantity - item.Quantity });
                }
            }

            return newOrderId;
        }
        catch (DO.AlreadyExistsIdException ex)
        {
            // המרה של חריגת DAL לחריגת BL תואמת
            throw new BLAlreadyExistsException(order.Id, "Order", ex);
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BLIdNotFoundException(order.Id, "Order", ex);
        }
        catch (Exception ex)
        {
            // עטיפת שגיאות לא צפויות (כמו בעיות ב-XML) בשגיאת תהליך כללית
            throw new BLOrderProcessException("An error occurred while saving the order to the database. Please try again.", ex);
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
                let boOrd = Tools.ToBo(doOrd, _dal.OrderItem.ReadAllByOrder(doOrd.Id))
                where filter == null || filter(boOrd)
                select boOrd).ToList();
    }

    public Order? GetOrderDetails(int orderId)
    {
        var doOrder = _dal.Order.Read(orderId);
        if (doOrder == null) return null;

        // שליפת הפריטים השייכים להזמנה כדי להחזיר אובייקט BO שלם
        var items = _dal.OrderItem.ReadAllByOrder(orderId);
        return Tools.ToBo(doOrder, items);
    }
}