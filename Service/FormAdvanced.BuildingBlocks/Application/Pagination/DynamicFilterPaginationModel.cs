using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.BuildingBlocks.Application.Configuration.Pagination
{
    public class DynamicFilterPaginationModel
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public FilterModel Filter { get; set; }
        public SortModel OrderBy { get; set; }
    }
}
