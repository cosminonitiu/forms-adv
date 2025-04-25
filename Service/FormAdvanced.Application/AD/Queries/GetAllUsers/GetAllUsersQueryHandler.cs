using FormAdvanced.Application.Common.Caching;
using FormAdvanced.Application.FormRequests.Queries.GetFormRequest;
using FormAdvanced.Domain.Entities;
using FormAdvanced.Domain.Interfaces;
using FormAdvanced.Infrastructure.Services.AzureAD;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.AD.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<ADUser>>
    {
        private readonly IAzureADService _adService;

        public GetAllUsersQueryHandler(IAzureADService adService)
        {
            _adService = adService;
        }

        public async Task<List<ADUser>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _adService.GetAllUsersAsync();
        }
    }
}
