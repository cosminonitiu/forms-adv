
using Azure.Identity;
using FormAdvanced.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace FormAdvanced.Infrastructure.Services.AzureAD
{
    public class AzureADService : IAzureADService
    {
        private readonly AzureADOptions _options;
        private readonly GraphServiceClient graphClient;
        private static string[] scopes = { "https://graph.microsoft.com/.default" }; // { "User.Read" };

        public AzureADService(IOptions<AzureADOptions> options)
        {
            _options = options.Value;
            graphClient = GetAuthenticatedGraphClient(scopes);
        }

        public GraphServiceClient GetAuthenticatedGraphClient(string[] scopes)
        {
            var clientSecretCredential = new ClientSecretCredential(
                _options.TenantId, _options.ClientId, _options.SecretId);
            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
            return graphClient;
        }

        public async Task<List<ADUser>> GetAllUsersAsync()
        {
            var users = await graphClient.Users.GetAsync();
            if(users != null && users.Value != null) {
                return users.Value.Select(u => MapUserToModel(u)).ToList();
            } else
            {
                return new List<ADUser>();
            }
        }


        public async Task<ADUser?> GetUserManagerAsync(string userId)
        {
            // The Manager endpoint returns directory objects; here we cast it to a User.
            var manager = await graphClient.Users[userId].Manager
                .GetAsync();

            var managerUser = manager as Microsoft.Graph.Models.User;
            if(managerUser != null)
            {
                return MapUserToModel(managerUser);
            }
            return null;
        }

        public async Task<List<string>> GetUserLocationAsync(string userId)
        {
            var user = await graphClient.Users[userId]
                .GetAsync();

            var locations = new List<string>();
            if (!string.IsNullOrEmpty(user.OfficeLocation))
            {
                locations.Add(user.OfficeLocation);
            }
            return locations;
        }

        public ADUser MapUserToModel(Microsoft.Graph.Models.User u)
        {
            return new ADUser(
                        Id: u.Id,
                        AppRoleAssignments: u.AppRoleAssignments,
                        AuthorizationInfo: u.AuthorizationInfo,
                        Country: u.Country,
                        Department: u.Department,
                        DirectReports: u.DirectReports,
                        DisplayName: u.DisplayName,
                        EmployeeId: u.EmployeeId,
                        EmployeeOrgData: u.EmployeeOrgData,
                        Identities: u.Identities,
                        JobTitle: u.JobTitle,
                        Mail: u.Mail,
                        Manager: u.Manager,
                        MemberOf: u.MemberOf,
                        MobilePhone: u.MobilePhone,
                        UserPrincipalName: u.UserPrincipalName
                        );
        }
    }
}
