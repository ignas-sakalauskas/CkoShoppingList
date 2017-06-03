using System;

namespace Checkout.ApiServices.SharedModels
{
    public class BasePagination
    {
        public int? Count { get; set; }
        public int? Offset { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PageSize { get; set; }
        public string PageNumber { get; set; }
    }
}
