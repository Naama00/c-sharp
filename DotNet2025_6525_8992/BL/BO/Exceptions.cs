using System;

namespace BL.BO;

[Serializable]
public class BLException : Exception
{
    public BLException(string? message) : base(message) { }
    public BLException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class BLIdNotFoundException : BLException
{
    public BLIdNotFoundException(string? message) : base(message) { }
    public BLIdNotFoundException(string message, Exception inner) : base(message, inner) { }

    public BLIdNotFoundException(int id, string entity)
        : base($"BL: The {entity} with ID {id} was not found.") { }

    public BLIdNotFoundException(int id, string entity, Exception inner)
        : base($"BL: The {entity} with ID {id} was not found.", inner) { }
}

[Serializable]
public class BLAlreadyExistsException : BLException
{
    public BLAlreadyExistsException(string? message) : base(message) { }
    public BLAlreadyExistsException(string message, Exception inner) : base(message, inner) { }

    public BLAlreadyExistsException(int id, string entity)
        : base($"BL: The {entity} with ID {id} already exists.") { }

    public BLAlreadyExistsException(int id, string entity, Exception inner)
        : base($"BL: The {entity} with ID {id} already exists.", inner) { }
}

[Serializable]
public class BLInvalidInputException : BLException
{
    public BLInvalidInputException(string? message) : base(message) { }
    public BLInvalidInputException(string message, Exception inner) : base(message, inner) { }
}
[Serializable]
public class BLOutOfStockException : BLException
{
    public int ProductId { get; }

    public BLOutOfStockException(string? message) : base(message) { }

    public BLOutOfStockException(int productId, string productName)
        : base($"BL: Product '{productName}' (ID: {productId}) is out of stock.")
    {
        ProductId = productId;
    }

    public BLOutOfStockException(int productId, string productName, Exception inner)
        : base($"BL: Product '{productName}' (ID: {productId}) is out of stock.", inner)
    {
        ProductId = productId;
    }
}

[Serializable]
public class BLNullPropertyException : BLException
{
    public BLNullPropertyException(string? message) : base(message) { }

    public BLNullPropertyException(string entity, string property)
        : base($"BL: The {property} of {entity} cannot be null or empty.") { }
}

[Serializable]
public class BLOrderProcessException : BLException
{
    public BLOrderProcessException(string? message) : base(message) { }
    public BLOrderProcessException(string message, Exception inner) : base(message, inner) { }
}
[Serializable]
public class BLDeletionException : BLException
{
    public BLDeletionException(string? message) : base(message) { }
    public BLDeletionException(string entity, int id, string reason)
        : base($"BL: Cannot delete {entity} with ID {id}. Reason: {reason}") { }
}

[Serializable]
public class BLDataValidationException : BLException
{
    public BLDataValidationException(string? message) : base(message) { }
    public BLDataValidationException(string entity, string property, string reason)
        : base($"BL: Validation failed for {entity}. Property '{property}' {reason}.") { }
}

[Serializable]
public class BLDatabaseException : BLException
{
    public BLDatabaseException(string message, Exception inner)
        : base($"BL: A data access error occurred: {message}", inner) { }
}