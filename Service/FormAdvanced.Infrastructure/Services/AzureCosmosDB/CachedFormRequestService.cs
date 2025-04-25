using FormAdvanced.Domain.Entities;
using FormAdvanced.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FormAdvanced.Infrastructure.Services.AzureCosmosDB
{
    public sealed class CachedFormRequestService : IFormRequestService
    {
        private readonly IFormRequestService _formRequestService;
        private readonly IMemoryCache _cache;
        private const string ALL_FORMS_CACHE_KEY = "all_forms";
        private const string OWNER_FORMS_CACHE_KEY_PREFIX = "owner_forms_";
        private const string FORM_BY_ID_CACHE_KEY_PREFIX = "form_by_id_";
        private static readonly TimeSpan CACHE_DURATION = TimeSpan.FromHours(1);

        public CachedFormRequestService(IFormRequestService formRequestService, IMemoryCache cache)
        {
            _formRequestService = formRequestService;
            _cache = cache;
        }

        public async Task<FormRequest?> GetByIdAsync(string id, string owner)
        {
            var cacheKey = $"{FORM_BY_ID_CACHE_KEY_PREFIX}{id}_{owner}";
            
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetAbsoluteExpiration(CACHE_DURATION);
                return await _formRequestService.GetByIdAsync(id, owner);
            });
        }

        public async Task<List<FormRequest>> GetByOwnerAsync(string owner)
        {
            var cacheKey = $"{OWNER_FORMS_CACHE_KEY_PREFIX}{owner}";
            
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetAbsoluteExpiration(CACHE_DURATION);
                return await _formRequestService.GetByOwnerAsync(owner);
            });
        }

        public async Task<List<FormRequest>> GetAllAsync()
        {
            return await _cache.GetOrCreateAsync(ALL_FORMS_CACHE_KEY, async entry =>
            {
                entry.SetAbsoluteExpiration(CACHE_DURATION);
                return await _formRequestService.GetAllAsync();
            });
        }

        public async Task<FormRequest> UpsertAsync(FormRequest formRequest)
        {
            var result = await _formRequestService.UpsertAsync(formRequest);
            
            // Invalidate relevant cache entries
            _cache.Remove(ALL_FORMS_CACHE_KEY);
            _cache.Remove($"{OWNER_FORMS_CACHE_KEY_PREFIX}{formRequest.Owner}");
            _cache.Remove($"{FORM_BY_ID_CACHE_KEY_PREFIX}{formRequest.id}_{formRequest.Owner}");
            
            return result;
        }

        public async Task DeleteAsync(string id, string owner)
        {
            await _formRequestService.DeleteAsync(id, owner);
            
            // Invalidate relevant cache entries
            _cache.Remove(ALL_FORMS_CACHE_KEY);
            _cache.Remove($"{OWNER_FORMS_CACHE_KEY_PREFIX}{owner}");
            _cache.Remove($"{FORM_BY_ID_CACHE_KEY_PREFIX}{id}_{owner}");
        }
    }
} 