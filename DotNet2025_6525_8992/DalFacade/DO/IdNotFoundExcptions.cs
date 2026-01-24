using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class IdNotFoundExcptions : Exception
    {

        public IdNotFoundExcptions(int id, string entityName)
            : base($"לא נמצא {entityName} עם מזהה {id}.")
        {
        }

        public IdNotFoundExcptions(int id, string entityName, Exception inner)
            : base($"לא נמצא {entityName} עם מזהה {id}.", inner)
        {
        }
    }


}

