using FormAdvanced.Application.Common.Caching;
using FormAdvanced.BuildingBlocks.Application.Configuration.Queries;
using FormAdvanced.Domain.Entities;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.AD.Queries.GetAllUsers
{
    public sealed record GetAllUsersQuery() : IQuery<List<ADUser>>;
}
