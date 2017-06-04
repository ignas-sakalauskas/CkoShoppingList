using System.Collections.Generic;

namespace Checkout.ApiServices.ShoppingList.ResponseModels
{
    public class DrinkList
    {
        public int Count { get; set; }
        public IList<Drink> Data { get; set; }
    }
}