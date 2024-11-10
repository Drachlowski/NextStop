using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Common.Exceptions;

public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string entityName, object key) 
        : base($"Entity '{entityName}' with key '{key}' already exists.") { }
}
