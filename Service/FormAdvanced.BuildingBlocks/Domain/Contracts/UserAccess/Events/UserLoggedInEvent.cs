using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.BuildingBlocks.Domain.Contracts.UserAccess.Events
{
    public record UserLoggedInEvent(string Username);
}
