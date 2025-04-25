using FormAdvanced.BuildingBlocks.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Domain.Exceptions
{
    public sealed class NotificationNotFoundException : NotFoundException
    {
        public NotificationNotFoundException(string id)
            : base($"The notification with the id " + id + " was not found")
        {
        }
    }
}
