namespace FormAdvanced.BuildingBlocks.Application.Configuration.Pagination
{
    public class PagedQueryResult<TEntity>
        where TEntity : class
    {
        public List<TEntity> Entities { get; set; }
        public int TotalEntities { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int Page => Take != 0 ? (Skip / Take) + 1 : 0;
        public int MaxPages => Take != 0 ? (int)Math.Ceiling((double)(TotalEntities / Take)) + 1 : 0;

        public PagedQueryResult()
        {
            
        }

        public PagedQueryResult(List<TEntity> entities)
        {
            this.Entities = entities;
        }
        
    }
}
