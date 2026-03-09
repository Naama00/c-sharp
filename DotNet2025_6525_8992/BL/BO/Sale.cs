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

    public override string ToString() => base.ToString();
}
