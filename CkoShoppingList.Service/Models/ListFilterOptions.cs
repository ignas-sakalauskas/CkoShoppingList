using System;

namespace CkoShoppingList.Service.Models
{
    public class ListFilterOptions
    {
        public int? Count { get; set; }
        public int? Offset { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}