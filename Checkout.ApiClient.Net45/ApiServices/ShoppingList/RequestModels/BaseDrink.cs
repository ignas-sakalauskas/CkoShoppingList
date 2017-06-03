using Newtonsoft.Json;

namespace Checkout.ApiServices.ShoppingList.RequestModels
{
    public class BaseDrink
    {
        [JsonProperty("key")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public int Quantity { get; set; }
    }
}
