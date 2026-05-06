

using BL.BlApi;

namespace BL.BlImplementation;

internal class Bl: BL.BlApi.IBl
{
    public IProduct Product => new ProductImplementation();
    public ICustomer Customer => new CustomerImplementation();
    public ISale Sale => new SaleImplementation();
    public IOrder Order => new OrderImplementation();
}
