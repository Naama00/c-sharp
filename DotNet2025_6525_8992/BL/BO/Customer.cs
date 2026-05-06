using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BL.BO;

public class Customer
{
    public int Id { get; init; }
    public string CustomerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsClubMember { get; set; } = false;
    public override string ToString() => $@"
    Customer ID:   {Id}
    Name:          {CustomerName}
    Phone:         {PhoneNumber}
    Address:       {Address}
    is Club Member: {IsClubMember}
    ---------------------------";
}
