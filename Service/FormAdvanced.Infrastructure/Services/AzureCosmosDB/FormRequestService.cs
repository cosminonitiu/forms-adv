using FormAdvanced.Domain.Entities;
using FormAdvanced.Domain.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace FormAdvanced.Infrastructure.Services.AzureCosmosDB
{
    public sealed class SubmittedRequestService : ISubmittedRequestService
    {
        private readonly Container _container;
        public SubmittedRequestService(CosmosClient client, IOptions<AzureCosmosDBConfiguration> configurationOptions)
        {
            var config = configurationOptions.Value;
            _container = client
                .GetDatabase(config.DatabaseName)
                .GetContainer(config.SubmittedContainerName);
        }

        public async Task<SubmittedRequest?> GetByIdAsync(string id, string owner)
        {
            try
            {
                var response = await _container.ReadItemAsync<SubmittedRequest>(
                    id,
                    new PartitionKey(owner)
                );

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<SubmittedRequest>> GetByOwnerAsync(string owner)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Owner = @owner")
                .WithParameter("@owner", owner);

            using var feed = _container.GetItemQueryIterator<SubmittedRequest>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(owner)
                });

            var results = new List<SubmittedRequest>();
            while (feed.HasMoreResults)
            {
                var response = await feed.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<List<SubmittedRequest>> GetByApproverAsync(string approver)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.ApproverUID = @approver")
                .WithParameter("@approver", approver); ;

            using var feed = _container.GetItemQueryIterator<SubmittedRequest>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    // Enable cross-partition queries
                    MaxItemCount = -1,
                    MaxConcurrency = -1
                });

            var results = new List<SubmittedRequest>();

            while (feed.HasMoreResults)
            {
                var response = await feed.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<SubmittedRequest> UpsertAsync(SubmittedRequest formRequest)
        {
            var response = await _container.UpsertItemAsync(
                formRequest,
                new PartitionKey(formRequest.Owner)
            );

            return response.Resource;
        }

        public async Task DeleteAsync(string id, string owner)
        {
            await _container.DeleteItemAsync<SubmittedRequest>(
                id,
                new PartitionKey(owner)
            );
        }
    }
}
