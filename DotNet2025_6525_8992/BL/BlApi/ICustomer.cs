
using System;
using System.Collections.Generic;
using BL.BO;

namespace BL.BlApi;

public interface ICustomer
{
    int Create(Customer item);
    Customer? Read(int id);
    Customer? Read(Func<Customer, bool> filter);
    List<Customer> ReadAll(Func<Customer, bool>? filter = null);
    void Update(Customer item);
    void Delete(int id);
}