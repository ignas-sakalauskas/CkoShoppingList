using System;
using Checkout;
using Checkout.ApiServices.ShoppingList.RequestModels;
using Checkout.ApiServices.ShoppingList.ResponseModels;

namespace CkoShoppingList.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var ckoApiClient = new APIClient("sk_test_32b9cb39-1cd6-4f86-b750-7069a133667d", Checkout.Helpers.Environment.ShoppingListTest);
                var drinkCreateRequest = new DrinkCreate
                {
                    Name = "Coke",
                    Quantity = 123
                };

                var apiResponse = ckoApiClient.ShoppingListService.CreateDrink(drinkCreateRequest);
                Console.WriteLine(apiResponse.HasError
                    ? $"Error: {apiResponse.Error.Message}"
                    : $"Success: {apiResponse.HttpStatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            try
            {
                var ckoApiClient = new APIClient("sk_test_32b9cb39-1cd6-4f86-b750-7069a133667d", Checkout.Helpers.Environment.ShoppingListTest);
                var drinkGetListRequest = new DrinkGetList();
                var apiResponse = ckoApiClient.ShoppingListService.GetDrinkList(drinkGetListRequest);
                Console.WriteLine(apiResponse.HasError
                    ? $"Error: {apiResponse.Error.Message}"
                    : $"Success: {apiResponse.HttpStatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            try
            {
                var ckoApiClient = new APIClient("sk_test_32b9cb39-1cd6-4f86-b750-7069a133667d", Checkout.Helpers.Environment.ShoppingListTest);
                var apiResponse = ckoApiClient.ShoppingListService.GetDrink("Coke");
                Console.WriteLine(apiResponse.HasError
                    ? $"Error: {apiResponse.Error.Message}"
                    : $"Success: {apiResponse.HttpStatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            try
            {
                var ckoApiClient = new APIClient("sk_test_32b9cb39-1cd6-4f86-b750-7069a133667d", Checkout.Helpers.Environment.ShoppingListTest);
                var request = new DrinkUpdate
                {
                    Name = "Coke",
                    Quantity = 333
                };

                var apiResponse = ckoApiClient.ShoppingListService.UpdateDrink("Coke", request);
                Console.WriteLine(apiResponse.HasError
                    ? $"Error: {apiResponse.Error.Message}"
                    : $"Success: {apiResponse.HttpStatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            try
            {
                var ckoApiClient = new APIClient("sk_test_32b9cb39-1cd6-4f86-b750-7069a133667d", Checkout.Helpers.Environment.ShoppingListTest);
                var apiResponse = ckoApiClient.ShoppingListService.DeleteDrink("Coke");
                Console.WriteLine(apiResponse.HasError
                    ? $"Error: {apiResponse.Error.Message}"
                    : $"Success: {apiResponse.HttpStatusCode}");
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
