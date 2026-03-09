using System;
using System.Collections.Generic;
using System.Linq;
namespace BL.BlImplementation;

internal class CustomerImplementation : BL.BlApi.ICustomer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Customer item)
    {
        // convert BO -> DO explicitly using the Tools static methods (no using BO/DO)
        var doItem = BO.Tools.ToDo(item);

        try
        {
            // query-syntax usage #1: check duplicates by PhoneNumber before creating (example of a query expression)
            var duplicate = (from c in _dal.Customer.ReadAll()
                             where c != null && c.PhoneNumber == doItem.PhoneNumber
                             select c).FirstOrDefault();

            if (duplicate != null)
            {
                throw new BlAlreadyExistsException(duplicate.Id, "Customer");
            }

            return _dal.Customer.Create(doItem);
        }
        catch (DO.AlreadyExistsIdException ex)
        {
            // wrap DAL/DO exception in BL exception and rethrow
            throw new BlAlreadyExistsException(ex.HResult, "Customer", ex);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public BO.Customer? Read(int id)
    {
        try
        {
            var d = _dal.Customer.Read(id);
            return d == null ? null : BO.Tools.ToBo(d);
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(id, "Customer", ex);
        }
    }

    public BO.Customer? Read(Func<BO.Customer, bool> filter)
    {
        if (filter == null) return null;

        try
        {
            // query-syntax usage #2: project DO -> BO and find first that matches the BO predicate
            var result = (from d in _dal.Customer.ReadAll()
                          let b = BO.Tools.ToBo(d)
                          where b != null && filter(b)
                          select b).FirstOrDefault();

            return result;
        }
        catch (DO.IdNotFoundException ex)
        {
            // in case DAL throws DO.NotFound (defensive)
            throw new BlIdNotFoundException(-1, "Customer", ex);
        }
    }

    public List<BO.Customer> ReadAll(Func<BO.Customer, bool>? filter = null)
    {
        try
        {
            if (filter == null)
            {
                // extension-method usage #1: Select projection DO -> BO
                return _dal.Customer.ReadAll()
                           .Select(d => BO.Tools.ToBo(d))
                           .ToList();
            }

            // build a DO predicate by converting DO -> BO inside the predicate
            Func<DO.Customer, bool> doFilter = d => filter(BO.Tools.ToBo(d));

            var dalResult = _dal.Customer.ReadAll(doFilter);

            // extension-method usage #2: Where + Select projection
            return dalResult
                       .Where(d => d != null && filter(BO.Tools.ToBo(d)))
                       .Select(d => BO.Tools.ToBo(d))
                       .ToList();
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(-1, "Customer", ex);
        }
    }

    public void Update(BO.Customer item)
    {
        var doItem = BO.Tools.ToDo(item);

        try
        {
            _dal.Customer.Update(doItem);
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(item.Id, "Customer", ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            _dal.Customer.Delete(id);
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(id, "Customer", ex);
        }
    }

    // --- BL layer specific exceptions (wrapping DO exceptions) ---
    internal class BlException : Exception
    {
        public BlException(string message, Exception? inner = null) : base(message, inner) { }
    }

    internal class BlIdNotFoundException : BlException
    {
        public BlIdNotFoundException(int id, string entity, Exception? inner = null)
            : base($"BL: The {entity} with ID {id} was not found.", inner) { }
    }

    internal class BlAlreadyExistsException : BlException
    {
        public BlAlreadyExistsException(int id, string entity, Exception? inner = null)
            : base($"BL: The {entity} with ID {id} already exists.", inner) { }
    }
}