using System;
using DO;

namespace BL.BO;

internal static class Tools
{
    // Order conversions
    // Order conversions
    public static Order ToBo(this DO.Order d) =>
        new Order
        {
            Id = d.Id,
            CustomerId = d.CustomerId,
            OrderDate = d.OrderDate,
            TotalPrice = d.TotalPrice,
            Items = d.Items?.Select(i => i.ToBo()).ToList() ?? new List<OrderItem>()
        };

    public static DO.Order ToDo(this Order b) =>
        new DO.Order(
            b.Id,
            b.CustomerId,
            b.OrderDate,
            b.TotalPrice,
            b.Items?.Select(i => i.ToDo(b.Id)).ToList() ?? new List<DO.OrderItem>()
        );

    // OrderItem conversions
    public static OrderItem ToBo(this DO.OrderItem d) =>
        new OrderItem
        {
            ProductId = d.ProductId,
            ProductName = "", // ניתן לשלוף את השם מה-Product במידת הצורך ב-Service
            Quantity = d.Quantity,
            PricePerUnit = d.PricePerUnit
        };

    // המרה של פריט בודד ל-DO (בדרך כלל דורש גם את ה-OrderId)
    public static DO.OrderItem ToDo(this OrderItem b, int orderId) =>
        new DO.OrderItem(orderId, b.ProductId, b.Quantity, b.PricePerUnit);
    // Product conversions
    public static Product ToBo(this DO.Product d) =>
        new Product
        {
            Id = d.Id,
            Name = d.Name,
            Category = d.Category.ToBo(),
            Price = d.Price,
            Quantity = d.Quantity
        };

    public static DO.Product ToDo(this Product b) =>
        new DO.Product(b.Id, b.Name, b.Category.ToDo(), b.Price, b.Quantity);

    // Customer conversions
    public static Customer ToBo(this DO.Customer d) =>
        new Customer
        {
            Id = d.Id,
            CustomerName = d.CustomerName,
            Address = d.Address,
            PhoneNumber = d.PhoneNumber,
            IsClubMember = d.IsClubMember
        };

    public static DO.Customer ToDo(this Customer b) =>
        new DO.Customer(b.Id, b.CustomerName, b.Address, b.PhoneNumber, b.IsClubMember);

    // Sale conversions
    public static Sale ToBo(this DO.Sale d) =>
        new Sale
        {
            Id = d.Id,
            ProductId = d.ProductId,
            RequiredQuantity = d.RequiredQuantity,
            DiscountedPrice = d.DiscountedPrice,
            IsForClubMembers = d.IsForClubMembers,
            SaleStartDate = d.SaleStartDate,
            SaleEndDate = d.SaleEndDate
        };

    public static DO.Sale ToDo(this Sale b) =>
        new DO.Sale(b.Id, b.ProductId, b.RequiredQuantity, b.DiscountedPrice, b.IsForClubMembers, b.SaleStartDate, b.SaleEndDate);

    // Enum conversions (safe direct casts because enum values match)
    public static Categories ToBo(this DO.Categories c) => (Categories)c;
    public static DO.Categories ToDo(this Categories c) => (DO.Categories)c;
}

