
namespace DO;


    public class AlreadyExistsIdException : Exception
    {
        public AlreadyExistsIdException(int id, string entity)
            : base($"The {entity} with ID {id} already exists.") { }
    }

    public class IdNotFoundException : Exception
    {
        public IdNotFoundException(int id, string entity) 
            : base($"The {entity} with ID {id} was not found.") { }
    }

  
