using System.Collections.Generic;

namespace Checkout.ApiServices.ShoppingList.ResponseModels
{
    public class DrinkList
    {
        public int Count{ get; set; }
        public List<Drink> Data { get; set; }
    }
}