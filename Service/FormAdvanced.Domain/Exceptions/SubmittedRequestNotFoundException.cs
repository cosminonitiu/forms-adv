using FormAdvanced.BuildingBlocks.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Domain.Exceptions
{
    public sealed class NotTheApproverException : ConflictException
    {
        public NotTheApproverException()
            : base($"You are not the approver of the request")
        {
        }
    }
}
