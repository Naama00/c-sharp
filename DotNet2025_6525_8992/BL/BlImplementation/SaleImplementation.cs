using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.BlImplementation;

internal class SaleImplementation : BL.BlApi.ISale
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Sale item)
    {
        var doItem = BO.Tools.ToDo(item);

        try
        {
            // query-syntax usage #1: check overlapping sale for same product and date range
            var overlapping = (from s in _dal.Sale.ReadAll()
                               where s != null && s.ProductId == doItem.ProductId
                                     && !(doItem.SaleEndDate <= s.SaleStartDate || doItem.SaleStartDate >= s.SaleEndDate)
                               select s).FirstOrDefault();

            if (overlapping != null)
            {
                throw new BlAlreadyExistsException(overlapping.Id, "Sale");
            }

            return _dal.Sale.Create(doItem);
        }
        catch (DO.AlreadyExistsIdException ex)
        {
            throw new BlAlreadyExistsException(ex.HResult, "Sale", ex);
        }
    }

    public BO.Sale? Read(int id)
    {
        try
        {
            var d = _dal.Sale.Read(id);
            return d == null ? null : BO.Tools.ToBo(d);
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(id, "Sale", ex);
        }
    }

    public BO.Sale? Read(Func<BO.Sale, bool> filter)
    {
        if (filter == null) return null;

        try
        {
            // extension-method usage #1: project DO -> BO then apply predicate (lambda)
            return _dal.Sale.ReadAll()
                       .Select(d => BO.Tools.ToBo(d))
                       .FirstOrDefault(b => b != null && filter(b));
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(-1, "Sale", ex);
        }
    }

    public List<BO.Sale> ReadAll(Func<BO.Sale, bool>? filter = null)
    {
        try
        {
            if (filter == null)
            {
                // extension-method usage #2: projection
                return _dal.Sale.ReadAll()
                           .Select(d => BO.Tools.ToBo(d))
                           .ToList();
            }

            // build DO predicate by converting DO->BO inside predicate (lambda)
            Func<DO.Sale, bool> doFilter = d => filter(BO.Tools.ToBo(d));

            // query-syntax usage #2: filter then convert
            var dalResult = from d in _dal.Sale.ReadAll(doFilter)
                            where d != null
                            select BO.Tools.ToBo(d);

            return dalResult.ToList();
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(-1, "Sale", ex);
        }
    }

    public void Update(BO.Sale item)
    {
        var doItem = BO.Tools.ToDo(item);

        try
        {
            _dal.Sale.Update(doItem);
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(item.Id, "Sale", ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            _dal.Sale.Delete(id);
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(id, "Sale", ex);
        }
    }

    // --- BL layer exceptions (wrapping DO exceptions) ---
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