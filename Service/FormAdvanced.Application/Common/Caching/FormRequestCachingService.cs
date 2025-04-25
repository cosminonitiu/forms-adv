using FormAdvanced.Domain.Entities;
using FormAdvanced.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormAdvanced.Application.Common.Caching
{
    public class FormRequestCachingService : BaseCachingService
    {
        private readonly IFormRequestService _formRequestService;
        private const string ALL_FORMS_CACHE_KEY = "all_forms";
        private const string OWNER_FORMS_CACHE_KEY_PREFIX = "owner_forms_";
        private const string FORM_BY_ID_CACHE_KEY_PREFIX = "form_by_id_";

        public FormRequestCachingService(
            IFormRequestService formRequestService,
            IMemoryCache cache,
            TimeSpan cacheDuration) : base(cache, cacheDuration)
        {
            _formRequestService = formRequestService;
        }

        public async Task<CachedResponse<List<Domain.Entities.FormRequest>>> GetAllAsync()
        {
            return await GetOrSetAsync(ALL_FORMS_CACHE_KEY, () => _formRequestService.GetAllAsync());
        }

        public async Task<CachedResponse<List<Domain.Entities.FormRequest>>> GetByOwnerAsync(string owner)
        {
            var cacheKey = $"{OWNER_FORMS_CACHE_KEY_PREFIX}{owner}";
            return await GetOrSetAsync(cacheKey, () => _formRequestService.GetByOwnerAsync(owner));
        }

        public async Task<CachedResponse<Domain.Entities.FormRequest?>> GetByIdAsync(string id, string owner)
        {
            var cacheKey = $"{FORM_BY_ID_CACHE_KEY_PREFIX}{id}_{owner}";
            return await GetOrSetAsync(cacheKey, () => _formRequestService.GetByIdAsync(id, owner));
        }

        public async Task<Domain.Entities.FormRequest> UpsertAsync(Domain.Entities.FormRequest formRequest)
        {
            var result = await _formRequestService.UpsertAsync(formRequest);
            
            // Invalidate relevant cache entries
            InvalidateCache(ALL_FORMS_CACHE_KEY);
            InvalidateCache($"{OWNER_FORMS_CACHE_KEY_PREFIX}{formRequest.Owner}");
            InvalidateCache($"{FORM_BY_ID_CACHE_KEY_PREFIX}{formRequest.id}_{formRequest.Owner}");
            
            return result;
        }

        public async Task DeleteAsync(string id, string owner)
        {
            await _formRequestService.DeleteAsync(id, owner);
            
            // Invalidate relevant cache entries
            InvalidateCache(ALL_FORMS_CACHE_KEY);
            InvalidateCache($"{OWNER_FORMS_CACHE_KEY_PREFIX}{owner}");
            InvalidateCache($"{FORM_BY_ID_CACHE_KEY_PREFIX}{id}_{owner}");
        }
    }
} 