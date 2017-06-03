using Checkout.ApiServices.ShoppingList.RequestModels;

namespace Checkout.ApiServices.ShoppingList.ResponseModels
{
    public class Drink:BaseDrink
    {
        public string Id { get; set; }
        public string Created { get; set; } // TODO needed?
    }
}
