using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.BuildingBlocks.Domain.Exceptions
{
    public abstract class ConflictException : ApplicationException
    {
        protected ConflictException(string message)
            : base("Conflict", message)
        {
        }
    }
}
