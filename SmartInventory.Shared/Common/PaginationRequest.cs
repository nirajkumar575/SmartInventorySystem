using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Shared.Common
{
    public class PaginationRequest
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public string? SortBy { get; set; }

        public string SortOrder { get; set; } = "asc";
    }
}
