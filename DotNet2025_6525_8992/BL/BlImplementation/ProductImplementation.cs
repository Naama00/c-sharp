using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.BlImplementation;

internal class ProductImplementation : BL.BlApi.IProduct
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Product item)
    {
        var doItem = BO.Tools.ToDo(item);

        try
        {
            // query-syntax usage #1: check duplicate by name/category
            var duplicate = (from p in _dal.Product.ReadAll()
                             where p != null && p.Name == doItem.Name && p.Category == doItem.Category
                             select p).FirstOrDefault();

            if (duplicate != null)
            {
                throw new BlAlreadyExistsException(duplicate.Id, "Product");
            }

            return _dal.Product.Create(doItem);
        }
        catch (DO.AlreadyExistsIdException ex)
        {
            throw new BlAlreadyExistsException(ex.HResult, "Product", ex);
        }
    }

    public BO.Product? Read(int id)
    {
        try
        {
            var d = _dal.Product.Read(id);
            return d == null ? null : BO.Tools.ToBo(d);
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(id, "Product", ex);
        }
    }

    public BO.Product? Read(Func<BO.Product, bool> filter)
    {
        if (filter == null) return null;

        try
        {
            // extension-method usage #1: project DO -> BO then apply predicate (lambda)
            return _dal.Product.ReadAll()
                       .Select(d => BO.Tools.ToBo(d))
                       .FirstOrDefault(b => b != null && filter(b));
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(-1, "Product", ex);
        }
    }

    public List<BO.Product> ReadAll(Func<BO.Product, bool>? filter = null)
    {
        try
        {
            if (filter == null)
            {
                // extension-method usage #2: projection
                return _dal.Product.ReadAll()
                           .Select(d => BO.Tools.ToBo(d))
                           .ToList();
            }

            // build DO predicate by converting DO->BO inside predicate (lambda)
            Func<DO.Product, bool> doFilter = d => filter(BO.Tools.ToBo(d));

            // query-syntax usage #2: get matching DOs then convert
            var dalResult = from d in _dal.Product.ReadAll(doFilter)
                            where d != null
                            select BO.Tools.ToBo(d);

            return dalResult.ToList();
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(-1, "Product", ex);
        }
    }

    public void Update(BO.Product item)
    {
        var doItem = BO.Tools.ToDo(item);

        try
        {
            _dal.Product.Update(doItem);
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(item.Id, "Product", ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            _dal.Product.Delete(id);
        }
        catch (DO.IdNotFoundException ex)
        {
            throw new BlIdNotFoundException(id, "Product", ex);
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