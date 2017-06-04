using System;
using System.Configuration;
using Checkout;
using Checkout.ApiServices.ShoppingList.RequestModels;
using IdentityModel.Client;

namespace CkoShoppingList.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var tokenHeader = GetBearerTokenHeader();

                TestShoppingListEndpoints(tokenHeader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void TestShoppingListEndpoints(string bearerToken)
        {
            var ckoApiClient = new APIClient(bearerToken, Checkout.Helpers.Environment.ShoppingListTest);

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

            Console.WriteLine("[INFO] All tests done!");
        }

        private static string GetBearerTokenHeader()
        {
            var client = new TokenClient(
                ConfigurationManager.AppSettings["AuthorityUri"],
                ConfigurationManager.AppSettings["ClientId"],
                ConfigurationManager.AppSettings["ClientSecret"]);

            var response = client.RequestClientCredentialsAsync(ConfigurationManager.AppSettings["Scope"]).Result;
            if (response.IsError)
            {
                throw new Exception($"Error retrieving access token from identity server: {response.Error}");
            }

            return $"Bearer {response.AccessToken}";
        }
    }
}
