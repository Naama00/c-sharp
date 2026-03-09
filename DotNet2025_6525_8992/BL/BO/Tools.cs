using System;
using DO;

namespace BL.BO;

internal static class Tools
{
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
            PhoneNumber = d.PhoneNumber
        };

    public static DO.Customer ToDo(this Customer b) =>
        new DO.Customer(b.Id, b.CustomerName, b.Address, b.PhoneNumber);

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