using SmartInventory.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Shared.QueryParameters
{
    public class ProductQueryParameters : PaginationRequest
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Search { get; set; } = string.Empty;
    }
}
