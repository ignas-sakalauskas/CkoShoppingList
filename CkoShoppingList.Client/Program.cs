using System;
using Checkout;
using Checkout.ApiServices.ShoppingList.RequestModels;

namespace CkoShoppingList.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // TODO get the actual secret key
                var ckoApiClient = new APIClient("sk_test_32b9cb39-1cd6-4f86-b750-7069a133667d", Checkout.Helpers.Environment.ShoppingListTest);

                Console.WriteLine("[INFO] Making CREATE request.");
                var resp1 = ckoApiClient.ShoppingListService.CreateDrink(new DrinkCreate
                {
                    Name = "Coke",
                    Quantity = 123
                });

                Console.WriteLine("[INFO] Making GET request.");
                var resp2 = ckoApiClient.ShoppingListService.GetDrinkList(new DrinkGetList());

                Console.WriteLine("[INFO] Making GET BY ID request.");
                var resp3 = ckoApiClient.ShoppingListService.GetDrink("Coke");

                Console.WriteLine("[INFO] Making UPDATE request.");
                var resp4 = ckoApiClient.ShoppingListService.UpdateDrink("Coke", new DrinkUpdate
                {
                    Name = "Coke",
                    Quantity = 333
                });

                Console.WriteLine("[INFO] Making DELETE request.");
                var resp5 = ckoApiClient.ShoppingListService.DeleteDrink("Coke");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            Console.WriteLine("done! any key to boom!");
            Console.ReadKey();
        }
    }
}
