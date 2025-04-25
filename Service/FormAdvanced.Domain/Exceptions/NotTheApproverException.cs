using FormAdvanced.BuildingBlocks.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Domain.Exceptions
{
    public sealed class SubmittedRequestNotFoundException : NotFoundException
    {
        public SubmittedRequestNotFoundException(string id)
            : base($"The submitted request with the id " + id + " was not found")
        {
        }
    }
}
