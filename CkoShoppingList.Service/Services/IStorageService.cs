using System.Collections.Generic;
using CkoShoppingList.Service.Models;

namespace CkoShoppingList.Service.Services
{
    public interface IStorageService
    {
        IList<KeyValuePair<string, int>> GetDrinks(ListFilterOptions filterOptions = null);
        KeyValuePair<string, int> GetDrink(string name);
        KeyValuePair<string, int> AddDrink(KeyValuePair<string, int> drink);
        KeyValuePair<string, int> UpdateDrink(string name, KeyValuePair<string, int> drink);
        void DeleteDrink(string name);
    }
}