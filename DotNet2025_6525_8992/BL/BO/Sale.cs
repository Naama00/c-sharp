using System;

namespace BL.BO;

public class Sale
{
    public int Id { get; init; }
    public int ProductId { get; set; }
    public int RequiredQuantity { get; set; }
    public double DiscountedPrice { get; set; }
    public bool IsForClubMembers { get; set; }
    public DateTime SaleStartDate { get; set; }
    public DateTime SaleEndDate { get; set; }

    public override string ToString() => $@"
    Sale ID:             {Id}
    Product ID:          {ProductId}
    Required Quantity:   {RequiredQuantity}
    Discounted Price:    {DiscountedPrice}
    Club Members Only:   {(IsForClubMembers ? "Yes" : "No")}
    Start Date:          {SaleStartDate:d}
    End Date:            {SaleEndDate:d}
    ------------------------------------------";
}
