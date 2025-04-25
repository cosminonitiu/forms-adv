namespace FormAdvanced.Application.Common.Caching
{
    public class CachedResponse<T>
    {
        public T Data { get; }
        public bool IsFromCache { get; }
        public DateTime? CachedAt { get; }

        public CachedResponse(T data, bool isFromCache, DateTime? cachedAt = null)
        {
            Data = data;
            IsFromCache = isFromCache;
            CachedAt = cachedAt;
        }
    }
} 