using FormAdvanced.Domain.Entities;
using FormAdvanced.Domain.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace FormAdvanced.Infrastructure.Services.AzureCosmosDB
{
    public sealed class NotificationsService : INotificationsService
    {
        private readonly Container _container;
        public NotificationsService(CosmosClient client, IOptions<AzureCosmosDBConfiguration> configurationOptions)
        {
            var config = configurationOptions.Value;
            _container = client
                .GetDatabase(config.DatabaseName)
                .GetContainer(config.NotificationsContainerName);
        }

        public async Task<Notification?> GetByIdAsync(string id, string owner)
        {
            try
            {
                var response = await _container.ReadItemAsync<Notification>(
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

        public async Task<List<Notification>> GetByOwnerAsync(string owner)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Owner = @owner")
                .WithParameter("@owner", owner);

            using var feed = _container.GetItemQueryIterator<Notification>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(owner)
                });

            var results = new List<Notification>();
            while (feed.HasMoreResults)
            {
                var response = await feed.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<Notification> UpsertAsync(Notification formRequest)
        {
            var response = await _container.UpsertItemAsync(
                formRequest,
                new PartitionKey(formRequest.Owner)
            );

            return response.Resource;
        }

        public async Task DeleteAsync(string id, string owner)
        {
            await _container.DeleteItemAsync<Notification>(
                id,
                new PartitionKey(owner)
            );
        }
    }
}
