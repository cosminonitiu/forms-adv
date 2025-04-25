using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Infrastructure.Services.AzureCosmosDB
{
    public record AzureCosmosDBConfiguration
    {
        public required string Endpoint { get; init; }
        public required string AccountKey { get; init; }
        public required string DatabaseName { get; init; }
        public required string FormContainerName { get; init; }
        public required string SubmittedContainerName { get; init; }
        public required string NotificationsContainerName { get; init; }
    }
}
