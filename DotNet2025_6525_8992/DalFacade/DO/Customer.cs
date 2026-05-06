
    namespace DO
{
    public record Customer(int Id, string CustomerName, string Address, string PhoneNumber, bool IsClubMember)
    {
        
        public Customer() : this(0, string.Empty, string.Empty, string.Empty, false)
        {
        }
    }
}