using Azure.Identity;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormAdvanced.Domain.Entities;

namespace FormAdvanced.Infrastructure.Services.AzureAD
{
    public interface IAzureADService
    {
        GraphServiceClient GetAuthenticatedGraphClient(string[] scopes);
        Task<List<ADUser>> GetAllUsersAsync();
        Task<ADUser?> GetUserManagerAsync(string userId);
    }
}