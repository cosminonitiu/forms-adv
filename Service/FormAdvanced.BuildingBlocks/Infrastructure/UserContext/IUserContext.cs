using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.BuildingBlocks.Infrastructure.UserContext
{
    public interface IUserContext
    {
        string UserId { get; }
        string Name { get; }
    }
}
