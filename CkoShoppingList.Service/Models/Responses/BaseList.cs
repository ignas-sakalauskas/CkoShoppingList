using System.Collections.Generic;

namespace CkoShoppingList.Service.Model
{
    public class BaseList<T>
    {
        public int Count { get; set; }
        public IList<T> Data { get; set; }
    }
}
