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
