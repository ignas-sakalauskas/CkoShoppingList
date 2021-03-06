using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.ShoppingList.RequestModels;
using Checkout.ApiServices.ShoppingList.ResponseModels;
using Checkout.Utilities;

namespace Checkout.ApiServices.ShoppingList
{
    public class ShoppingListService
    {
        public HttpResponse<Drink> CreateDrink(DrinkCreate requestModel)
        {
            return new ApiHttpClient().PostRequest<Drink>(ApiUrls.Drinks, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<Drink> UpdateDrink(string drinkId, DrinkUpdate requestModel)
        {
            var updateDrinkUri = string.Format(ApiUrls.Drink, drinkId);
            return new ApiHttpClient().PutRequest<Drink>(updateDrinkUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<OkResponse> DeleteDrink(string drinkId)
        {
            var deleteDrinkUri = string.Format(ApiUrls.Drink, drinkId);
            return new ApiHttpClient().DeleteRequest<OkResponse>(deleteDrinkUri, AppSettings.SecretKey);
        }

        public HttpResponse<Drink> GetDrink(string drinkId)
        {
            var getDrinkUri = string.Format(ApiUrls.Drink, drinkId);
            return new ApiHttpClient().GetRequest<Drink>(getDrinkUri, AppSettings.SecretKey);
        }

        public HttpResponse<DrinkList> GetDrinkList(DrinkGetList request)
        {
            var getDrinkListUri = ApiUrls.Drinks;

            if (request.Count.HasValue)
            {
                getDrinkListUri = UrlHelper.AddParameterToUrl(getDrinkListUri, "count", request.Count.ToString());
            }

            if (request.Offset.HasValue)
            {
                getDrinkListUri = UrlHelper.AddParameterToUrl(getDrinkListUri, "offset", request.Offset.ToString());
            }

            return new ApiHttpClient().GetRequest<DrinkList>(getDrinkListUri, AppSettings.SecretKey);
        }
    }
}