using FormAdvanced.BuildingBlocks.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Domain.Exceptions
{
    public sealed class UserManagerNotFoundException : NotFoundException
    {
        public UserManagerNotFoundException()
            : base($"No manager was found for your user")
        {
        }
    }
}
