using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.BuildingBlocks.Application.Configuration.Pagination
{
    public class FilterModel
    {
        public string ColIds { get; set; } // Column Ids split by ,
        public string? Filter { get; set; }

        public FilterModel(string ColIds, string Filter)
        {
            this.ColIds = ColIds;
            this.Filter = Filter != null ? Filter : "";
        }

        public FilterModel()
        {
   
        }
    }
}
